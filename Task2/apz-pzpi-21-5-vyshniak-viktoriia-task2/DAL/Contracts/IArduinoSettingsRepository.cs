using Domain.Models;

namespace DAL.Contracts;
public interface IArduinoSettingsRepository
{
    ArduinoSettings GetByPetId(Guid petId);

    void SetPetDeviceIp(string ipAddress, Guid arduinoSettingsId);
}
