using System.Security.Claims;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TagController(ITagService tagService, IUserService userService) : ControllerBase
{
    private readonly ITagService _tagService = tagService;
    private readonly IUserService _userService = userService;

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> addAnalogInput([FromBody] AnalogInputDTO analogInputDTO)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null) await _tagService.AddAnalogInput(new AnalogInput(analogInputDTO), Guid.Parse(userId));
            return Ok();
        }

        return Forbid("Authentication error!");
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> addDigitalInput([FromBody] DigitalInputDTO digitalInputDTO)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
                await _tagService.AddDigitalInput(new DigitalInput(digitalInputDTO), Guid.Parse(userId));
            return Ok();
        }

        return Forbid("Authentication error!");
    }


    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("{id}")]
    public async Task<ActionResult> deleteDigitalInput(Guid id)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null) await _tagService.DeleteDigitalInput(id, Guid.Parse(userId));
            return Ok();
        }

        return Forbid("Authentication error!");
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("{id}")]
    public async Task<ActionResult> deleteAnalogInput(Guid id)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null) await _tagService.DeleteAnalogInput(id, Guid.Parse(userId));
            return Ok();
        }

        return Forbid("Authentication error!");
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> switchTag([FromQuery] TagType type, [FromQuery] Guid tagId)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (type == TagType.Analog)
            {
                if (userId != null) await _tagService.SwitchAnalogTag(tagId, Guid.Parse(userId));
            }
            else if (type == TagType.Digital)
            {
                if (userId != null) await _tagService.SwitchDigitalTag(tagId, Guid.Parse(userId));
            }
            else
            {
                throw new InvalidInputException("Invalid tag type");
            }

            return Ok();
        }

        return Forbid("Authentication error!");
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult> getTags()
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                var user = await _userService.Get(Guid.Parse(userId));
                List<AnalogInputValueDTO> analogInputValueDTOs = new();
                foreach (var input in user?.AnalogInputs)
                    if (input != null)
                    {
                        var data = await _tagService.GetLatestAnalogTagValue(input.Id, user.Id);
                        analogInputValueDTOs.Add(new AnalogInputValueDTO(input, data?.Value ?? 0));
                    }

                List<DigitalInputValueDTO> digitalInputValueDTOs = new();
                foreach (var input in user.DigitalInputs)
                {
                    var data = await _tagService.GetLatestDigitalTagValue(input.Id, user.Id);
                    digitalInputValueDTOs.Add(new DigitalInputValueDTO(input, data?.Value ?? 0));
                }

                return Ok(new TagsDTO(analogInputValueDTOs, digitalInputValueDTOs));
            }
        }

        return Forbid("Authentication error!");
    }

    [HttpGet]
    public async Task<ActionResult> startupCheck()
    {
        _ = _tagService.StartupCheck();
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateAnalogInput([FromBody] AddressValueDTO analogValue)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null) await _tagService.UpdateAnalog(analogValue.Id, analogValue.Value, Guid.Parse(userId));
            return Ok();
        }

        return Forbid("Authentication error!");
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> updateDigitalInput([FromBody] AddressValueDTO analogValue)
    {
        var result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            var identity = result.Principal.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _tagService.UpdateDigital(analogValue.Id, analogValue.Value, Guid.Parse(userId));
            return Ok();
        }

        return Forbid("Authentication error!");
    }
}