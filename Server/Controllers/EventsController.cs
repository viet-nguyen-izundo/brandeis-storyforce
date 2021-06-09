using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Core.Operations;
using StoryForce.Server.Services;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private EventService _eventService;

        public EventsController(EventService eventService)
        {
            this._eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> Get()
        {
            return await this._eventService.GetAsync();
        }

        private async Task Seed()
        {
            var events = new List<Event>
            {
                new Event
                {
                    Name = "Passover",
                    Year = 2021
                },
                new Event
                {
                    Name = "Purim",
                    Year = 2021
                },
                new Event
                {
                    Name = "Yom Kippur",
                    Year = 2021
                }
            };

            await this._eventService.CreateMultipleAsync(events);
        }
    }
}
