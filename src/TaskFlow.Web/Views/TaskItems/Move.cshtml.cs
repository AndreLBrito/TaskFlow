using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TaskFlow.Web.Views.TaskItems
{
    public class Move : PageModel
    {
        private readonly ILogger<Move> _logger;

        public Move(ILogger<Move> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}