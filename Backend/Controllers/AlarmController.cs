using System.Security.Claims;
using Backend.DTO;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AlarmController : ControllerBase
{
    private readonly IAlarmService _alarmService;
    private readonly ITagService _tagService;

    public AlarmController(IAlarmService alarmService, ITagService tagService)
    {
        _alarmService = alarmService;
        _tagService = tagService;
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> addAlarm([FromBody] AlarmDTO alarmDto)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            await _alarmService.MakeAlarm(Guid.Parse(userId), alarmDto);


            return Ok("Successfully created alarm!");
        }

        return Forbid("Authentication error!");
    }


    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> deleteAlarm([FromQuery] Guid tagId, [FromQuery] Guid alarmId)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            await _alarmService.DeleteAlarm(Guid.Parse(userId), alarmId, tagId);


            return Ok("Successfully removed alarm!");
        }

        return Forbid("Authentication error!");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> getByTag([FromQuery] Guid tagId)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var input = await _tagService.GetAnalogInput(tagId, Guid.Parse(userId));
            return Ok(input.Alarms);
        }

        return Forbid("Authentication error!");
    }
}