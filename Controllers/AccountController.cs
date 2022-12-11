using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using ChainStoreApi.Handler;
using System.Text;


namespace ChainStoreApi.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class accountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly JwtSettings jwtSettings;
        private readonly RefreshTokenGenerator _refereshTokenGenerator;
        public accountController(AccountService accountService, IOptions<JwtSettings>options,RefreshTokenGenerator refresh){
            _accountService = accountService;
            jwtSettings = options.Value;
            _refereshTokenGenerator = refresh;
            

        }
        // Hàm băm password
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }

        }
        // Hàm kiểm tra password khi băm
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        // Hàm tạo Token login
        private string CreateToken(Account account)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(this.jwtSettings.securitykey);
            var tokendesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {new Claim(ClaimTypes.Name, account.username!), new Claim(ClaimTypes.Role, account.role!)}),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenhandler.CreateToken(tokendesc);
            string jwt = tokenhandler.WriteToken(token);
            return jwt;
        }
        
        
        [HttpGet]
        public async Task<List<Account>> Get() => await _accountService.GetAccountAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Account>> Get(string id)
        {
            var account = await _accountService.GetAccountAsync(id);
            if(account is null)
            {
                return NotFound();
            }
            return account;
        }

        //GET Account => Login
        [HttpPost("login")]
        public async Task<ActionResult> Get([FromBody] AccountDTO accountDTO)
        {

            var account = await _accountService.GetAccountLogin(accountDTO.username!);
            if(account is null)
            {
                return BadRequest("Username không tồn tại");
            }
            else
            {
                if(VerifyPasswordHash(accountDTO.password!,account.password!,account.salt!))
                {
                    string token = CreateToken(account);
                    string result = "{\"token\" : \"" + token + "\" ,\"account\": {" 
                        + "\"id\" :\"" + account.id
                        + "\",\"username\" :\"" + account.username
                        +"\"}}";
                    return Ok(result);
                    
                }
                return NotFound();
            }
            
        }
    
    [HttpPost]
    public async Task<IActionResult> Post(AccountDTO accountDTO)
    {
        // Kiểm tra username tồn tại??
        var accountcheck = await _accountService.GetAccountLogin(accountDTO.username!);
        if(accountcheck is not null)
        {
            return BadRequest("Username đã tồn tại !");
        }
        Account account = new Account();
        CreatePasswordHash(accountDTO.password!, out byte[] passwordHash, out byte[] passwordSalt);
        account.create_at = accountDTO.create_at;
        account.password = passwordHash;
        account.salt  = passwordSalt;
        account.create_user = accountDTO.create_user;
        account.username = accountDTO.username;
        account.role = accountDTO.role;
        await _accountService.CreateAccountAsync(account);

        return CreatedAtAction(nameof(Get), new { id = account.id }, account);
    }

    // [HttpPut("{id:length(24)}")]
    // public async Task<IActionResult> Update(string id, AccountDTO accountupdate)
    // {
    //     var account = await _accountService.GetAccountAsync(id);

    //     if (account is null)
    //     {
    //         return NotFound();
    //     }
        
    //     account.id = id;
        
    //     account.update_at = accountupdate.update_at;
    //     account.update_user = accountupdate.update_user;

    //     await _accountService.UpdateAccountAsync(id, account);

    //     return NoContent();
    // }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var staff = await _accountService.GetAccountAsync(id);

        if (staff is null)
        {
            return NotFound();
        }

        await _accountService.RemoveAccountAsync(id);

        return NoContent();
    }

    }
