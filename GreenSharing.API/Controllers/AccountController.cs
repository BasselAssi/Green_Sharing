﻿using GreenSharing.API.Dtos;
using GreenSharing.API.Repositories.DataAccessLayer.Models;
using GreenSharing.API.Repositories.Interface;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenSharing.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AccountController));

        //Repositories For Tables/Models where we have Custom Methods others than CRUD (CreateAsync ReadAsync UpdateAsync DeleteAsync GetAllAsync)
        private readonly IAccountRepository _accountRepository;

        //Generic Repositories For Tables/Models where we do only CreateAsync ReadAsync UpdateAsync DeleteAsync GetAllAsync 
        private readonly IGenericRepository<AccountType> _accountTypeRepository;
        private readonly IGenericRepository<AccountLocation> _accountLocationRepository;

        //MANDTORY
        public AccountController(IAccountRepository accountRepository,
            IGenericRepository<AccountType> accountTypeRepository, 
            IGenericRepository<AccountLocation> accountLocationRepository)
        {
            _accountRepository = accountRepository;
            _accountTypeRepository = accountTypeRepository;
            _accountLocationRepository = accountLocationRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginDTO), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            Account account;
            try
            {
                account = await _accountRepository.LoginAsync(loginDto);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok(account);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Account), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Create([FromBody] Account account)
        {
            Account accountCreated = null;
            try
            {
                //1. Find the AccounType related
                //Farmer, Gleaner, BankFood
                AccountType accountType = await _accountTypeRepository.FindAsync(x => (x.Id == account.AccountTypeId || x.Name.ToLower() == account.AccountTypeName.ToLower()));
                if (accountType == null)
                {
                    //TODO: Throw ...l'accountType n'existe pas !                
                }
                else
                {
                    //2. Create the Account attaching its AccounType
                    account.AccountType = accountType;
                    var result = await _accountRepository.CreateAsync(account);

                    //3. If there is Any AccountLocation Specified and createe it
                    //Creates Location if provided !
                    if (account.AccountLocations.Any()) {
                        foreach (var location in account.AccountLocations) {
                            await _accountLocationRepository.CreateOrUpdateAsync(location);
                        }
                    }
                }
            }
            catch (Exception e)

            {
                return BadRequest(e);
            }

            return Ok(accountCreated);
        }

        // GET api/<AccountController>/5
        [HttpGet("{accountId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Account), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Get(Guid accountId)
        {
            Account account ;
            try
            {
                account = await _accountRepository.FindAsync(x => x.Id == accountId);
            }
            catch (Exception e)

            {
                return BadRequest(e);
            }

            return Ok(account);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<Account>), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> GetAll([FromBody] LoginDTO loginDto)
        {
            IEnumerable<Account> accounts = null;
            try
            {
                accounts = await _accountRepository.AllAsync();
            }
            catch (Exception e)

            {
                return BadRequest(e);
            }

            return Ok(accounts);
        }
    }
}
