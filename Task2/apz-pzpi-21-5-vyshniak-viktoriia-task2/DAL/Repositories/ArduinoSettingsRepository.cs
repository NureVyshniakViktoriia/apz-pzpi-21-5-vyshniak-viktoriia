using AutoMapper;
using Common.Resources;
using DAL.Contracts;
using DAL.DbContexts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;
public class ArduinoSettingsRepository : IArduinoSettingsRepository
{
    private readonly DbContextBase _dbContext;
    private readonly Lazy<IMapper> _mapper;

    private readonly DbSet<ArduinoSettings> _arduinoSettings;
    private readonly DbSet<Pet> _pets;

    public ArduinoSettingsRepository(
        DbContextBase dbContext,
        Lazy<IMapper> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;

        _arduinoSettings = dbContext.ArduinoSettings;
        _pets = dbContext.Pets;
    }

    public ArduinoSettings GetByPetId(Guid petId)
    {
        var arduinoSettings = _arduinoSettings.FirstOrDefault(s => s.PetId == petId)
            ?? throw new ArgumentException(Resources.Get("ARDUINO_SETTINGS_NOT_FOUND"));

        return arduinoSettings;
    }

    public void SetPetDeviceIp(string ipAddress, Guid arduinoSettingsId)
    {
        var arduinoSettings = _arduinoSettings.FirstOrDefault(s => s.PetId == arduinoSettingsId)
            ?? throw new ArgumentException(Resources.Get("ARDUINO_SETTINGS_NOT_FOUND"));

        arduinoSettings.PetDeviceIpAddress = ipAddress;
        _dbContext.Commit();
    }
}
