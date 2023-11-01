﻿using API.Attributes;
using API.IServices;
using Entities.Models.Requests.UserRequests;
using Entities.Models.Responses.UserResponses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserSecurityService _userSecurityService;
        private readonly IUserService _userService;
        public UserController(IUserSecurityService userSecurityService, IUserService userService)
        {
            _userSecurityService = userSecurityService;
            _userService = userService;
        }
        [EndpointAuthorize(AllowsAnonymous = true)]
        [HttpPost(Name = "Register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterRequest registerRequest)
        {
            await _userService.UserRegister(registerRequest);
            return Ok();
        }
        [EndpointAuthorize(AllowsAnonymous = true)]
        [HttpPost(Name = "Login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            LoginResponse response = await _userSecurityService.GenerateRefreshBearerToken(loginRequest.UserName, loginRequest.UserPassword);
            return Ok(response);
        }
        [EndpointAuthorize(AllowsAnonymous = true)]
        [HttpPost(Name = "GenerateRefreshJWT")]
        public async Task<ActionResult<string>> GenerateRefreshJWT([FromBody] AccessTokenRequest refreshRequest)
        {
            var response = await _userSecurityService.GenerateAccessJWT(refreshRequest);
            return Ok(response);
        }
    }
}