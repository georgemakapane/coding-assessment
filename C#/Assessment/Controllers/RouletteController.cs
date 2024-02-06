using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouletteController : ControllerBase
    {
        private List<int> spinsHistory = new List<int>();
        private readonly Random random = new Random();

        /// <summary>
        /// Place a bet
        /// </summary>
        /// <param name="betNumber"></param>
        /// <param name="amount"></param>
        /// <returns></returns>

        [HttpPost("placebet")]
        public ActionResult PlaceBet(int betNumber, int amount)
        {
            if (betNumber < 0 || betNumber > 36)
                return BadRequest("Invalid bet number. Bet number must be between 0 and 36.");

            if (amount <= 0)
                return BadRequest("Invalid bet amount. Bet amount must be greater than 0.");

            return Ok($"Bet placed on number {betNumber} for {amount} credits.");
        }

        /// <summary>
        /// Stimulates spinning the roulette wheel
        /// </summary>
        /// <returns></returns>
        [HttpPost("spin")]
        public ActionResult<int> Spin()
        {
            int result = random.Next(0, 37);
            spinsHistory.Add(result);
            return Ok(result);
        }

        /// <summary>
        /// Returns the history of previous spins
        /// </summary>
        /// <returns></returns>
        [HttpGet("spinshistory")]
        public ActionResult<IEnumerable<int>> ShowPreviousSpins()
        {
            return Ok(spinsHistory);
        }

        /// <summary>
        /// Calculates the payout for a bet based
        /// </summary>
        /// <param name="winningNumber"></param>
        /// <param name="betAmount"></param>
        /// <returns></returns>
        [HttpPost("payout")]
        public ActionResult<int> Payout(int winningNumber, int betAmount)
        {
            if (spinsHistory.Count == 0)
                return BadRequest("No spins yet.");

            if (!spinsHistory.Contains(winningNumber))
                return BadRequest($"Winning number {winningNumber} has not been spun yet.");

            // Calculate the winning number
            if (winningNumber == spinsHistory.Last())
            {
                int payout = betAmount * 35;
                return Ok(payout);
            }
            else
            {
                return Ok(0);
            }
        }
    }
}