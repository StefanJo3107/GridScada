using System.Security.Claims;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ReportsController(IAlarmService alarmService, ITagService tagService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> allAlarmsByTime([FromQuery] DateTime timeFrom, [FromQuery] DateTime timeTo)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                List<AlarmReport> alarms = await alarmService.GetAllAlarmsByTime(Guid.Parse(userId), timeFrom, timeTo);

                return Ok(alarms);
            }
        }

        return Forbid("Authentication error!");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> allAlarmsByPriority([FromQuery] AlarmPriority alarmPriority)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                List<AlarmReport> alarms =
                    await alarmService.GetAllAlarmsByPriority(Guid.Parse(userId), alarmPriority);

                return Ok(alarms);
            }
        }

        return Forbid("Authentication error!");
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> allTagValuesByTime([FromQuery] DateTime timeFrom, [FromQuery] DateTime timeTo)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                var tagReports = await tagService.GetAllTagValuesByTime(Guid.Parse(userId), timeFrom, timeTo);

                return Ok(tagReports);
            }
        }

        return Forbid("Authentication error!");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> latestAnalogValues()
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                List<AnalogData?> tagReports = await tagService.GetLatestAnalogTagsValues(Guid.Parse(userId));

                return Ok(tagReports);
            }
        }

        return Forbid("Authentication error!");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> latestDigitalValues()
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                List<DigitalData?> tagReports = await tagService.GetLatestDigitalTagsValues(Guid.Parse(userId));

                return Ok(tagReports);
            }
        }

        return Forbid("Authentication error!");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> allAnalogTagValues([FromQuery] Guid tagId)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                List<AnalogData> tagReports = await tagService.GetAllAnalogTagValues(Guid.Parse(userId), tagId);

                return Ok(tagReports);
            }
        }

        return Forbid("Authentication error!");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> allDigitalTagValues([FromQuery] Guid tagId)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                List<DigitalData> tagReports = await tagService.GetAllDigitalTagValues(Guid.Parse(userId), tagId);

                return Ok(tagReports);
            }
        }

        return Forbid("Authentication error!");
    }
}