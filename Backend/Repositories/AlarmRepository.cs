using Backend.Database;
using Backend.Models;

namespace Backend.Repositories;

public class AlarmRepository(DatabaseContext context) : CrudRepository<Alarm>(context), IAlarmRepository;