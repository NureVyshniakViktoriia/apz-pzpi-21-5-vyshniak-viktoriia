using BLL.Contracts;
using BLL.Infrastructure.Commands;
using BLL.Infrastructure.Models.Arduino;
using Common.Enums;
using Common.Resources;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.IO.Ports;
using DAL.UnitOfWork;
using System.Globalization;

namespace BLL.Services;
public class ArduinoSettingsService : IArduinoSettingsService
{
    private readonly IInstitutionService _institutionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGoogleMapsLocationService _googleMapsLocationService;

    public ArduinoSettingsService(
        IInstitutionService institutionService,
        IUnitOfWork unitOfWork,
        IGoogleMapsLocationService googleMapsLocationService)
    {
        _institutionService = institutionService;
        _unitOfWork = unitOfWork;
        _googleMapsLocationService = googleMapsLocationService;
    }

    public ConfigureResponse ConfigurePetDevice(WifiSettingsModel wifiSettings, Guid arduinoSettingsId)
    {
        var command = new ConfigureCommand
        {
            CommandType = CommandType.ConfigurePetDevice,
            WifiSettings = wifiSettings,
        };

        var arduinoResponse = SendRequestBySerialPort<ConfigureResponse>(command);
        if (!arduinoResponse.IsSuccess)
            throw new ArgumentException(Resources.Get(arduinoResponse.ErrorCode));

        _unitOfWork.ArduinoSettings.Value.SetPetDeviceIp(arduinoResponse.IpAddress, arduinoSettingsId);

        return arduinoResponse;
    }

    public ConfigureResponse ConfigureRFIDReader(WifiSettingsModel wifiSettings, int rfidSettingsId)
    {
        var institution = _unitOfWork.Institutions.Value.GetById(rfidSettingsId);
        var command = new ConfigureRFIDReaderCommand
        {
            CommandType = CommandType.ConfigureRFIDReader,
            WifiSettings = wifiSettings,
            UserId = institution.OwnerId
        };

        var arduinoResponse = SendRequestBySerialPort<ConfigureResponse>(command);
        if (!arduinoResponse.IsSuccess)
            throw new ArgumentException(Resources.Get(arduinoResponse.ErrorCode));

        _unitOfWork.Institutions.Value.SetRFIDReaderIp(rfidSettingsId, arduinoResponse.IpAddress);

        return arduinoResponse;
    }

    public async Task<TemperatureMonitorResponse> GetPetCurrentTemperature(Guid petId)
    {
        var apiUrl = GetPetDeviceApiUrl(petId);
        var command = new CommandBase
        { 
            CommandType = CommandType.GetCurrentTemperature,
            Locale = CultureInfo.CurrentCulture.Name
        };

        var response = await ProcessCommand<TemperatureMonitorResponse>(command, apiUrl);
        return response;
    }

    public async Task<TemperatureMonitorResponse> GetPetAverageTemperature(Guid petId)
    {
        var apiUrl = GetPetDeviceApiUrl(petId);
        var command = new CommandBase
        { 
            CommandType = CommandType.GetAverageTemperature,
            Locale = CultureInfo.CurrentCulture.Name
        };

        var response = await ProcessCommand<TemperatureMonitorResponse>(command, apiUrl);
        return response;
    }

    public async Task<GPSTrackerResponse> GetPetCurrentLocation(Guid petId)
    {
        var command = new CommandBase { CommandType = CommandType.GetCurrentLocation };
        var apiUrl = GetPetDeviceApiUrl(petId);

        var response = await ProcessCommand<GPSTrackerResponse>(command, apiUrl);
        if (response.IsSuccess)
            response.AddressData = _googleMapsLocationService.GetLocation(response.Longtitude, response.Latitutde);

        return response;
    }

    #region Helpers

    private async Task<T> ProcessCommand<T>(
        CommandBase command,
        string apiUrl) where T : ArduinoResponseBase
    {
        try
        {
            var jsonCommand = JsonConvert.SerializeObject(command);
            var responseMessage = await SendHttpsRequest(jsonCommand, apiUrl);
            var arduinoResponse = await ProcessResponse<T>(responseMessage);

            return arduinoResponse;
        }
        catch (Exception ex)
        {
            return (T)new ArduinoResponseBase
            {
                IsSuccess = false
            };
        }
    }

    private async Task<HttpResponseMessage> SendHttpsRequest(string jsonData, string apiUrl)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        using var httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback
            = (sender, cert, chain, sslPolicyErrors) => true;

        using var client = new HttpClient(httpClientHandler);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(apiUrl, content);

        return response;
    }

    private async Task<T> ProcessResponse<T>(HttpResponseMessage responseMessage)
        where T : ArduinoResponseBase
    {
        var byteArray = await responseMessage.Content.ReadAsByteArrayAsync();
        var jsonString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
        var arduinoResponse = JsonConvert.DeserializeObject<T>(jsonString);

        return arduinoResponse;
    }

    private T SendRequestBySerialPort<T>(CommandBase command)
        where T : ArduinoResponseBase
    {
        var jsonCommand = JsonConvert.SerializeObject(command);
        var port = GetAvailablePort();
        try
        {
            port.Open();
            port.Write($"{jsonCommand}\n");

            return GetSerialPortResponse<T>(port);
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            port.Close();
        }
    }

    private T GetSerialPortResponse<T>(SerialPort port)
        where T : ArduinoResponseBase
    {
        var response = string.Empty;
        T arduinoResponse;

        while (!TryDeserializeResponse<T>(response, out arduinoResponse))
            response = port.ReadLine();

        return arduinoResponse;
    }

    private bool TryDeserializeResponse<T>(string response, out T result)
        where T : ArduinoResponseBase
    {
        try
        {
            result = JsonConvert.DeserializeObject<T>(response);
        }
        catch
        {
            result = null;
            return false;
        }

        return result != null && result.ErrorCode != null;
    }

    private SerialPort GetAvailablePort()
    {
        var serialPorts = SerialPort.GetPortNames();
        var port = new SerialPort(
            "COM3",
            9600,
            Parity.None,
            8,
            StopBits.One);

        return port;
    }

    private string GetRFIDApiUrl(int institutionId)
    {
        var settings = _institutionService.GetRFIDSettingsByInstitutionId(institutionId);
        var ipAddress = settings.RFIDReaderIpAddress;
        if (string.IsNullOrEmpty(ipAddress))
            throw new ArgumentException();

        var apiUrl = string.Format("https://{0}:443/data", ipAddress);
        return apiUrl;
    }

    private string GetPetDeviceApiUrl(Guid petId)
    {
        var settings = _unitOfWork.ArduinoSettings.Value.GetByPetId(petId);
        var ipAddress = settings.PetDeviceIpAddress;
        if (string.IsNullOrWhiteSpace(ipAddress))
            throw new ArgumentException();

        var apiUrl = string.Format("https://{0}:443/data", ipAddress);
        return apiUrl;
    }

    #endregion
}
