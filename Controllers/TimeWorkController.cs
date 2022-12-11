using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
namespace ChainStoreApi.Controllers;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

    [ApiController]
    [Route("[controller]")]
    public class timeworkController : ControllerBase
    {
        
        private readonly TimeWorkService _timeWorkService;
        public timeworkController(TimeWorkService timeWorkService)
        {
            _timeWorkService = timeWorkService;
        }
        // Get time work with store 

        // create time work
        
        // update time work 
    }