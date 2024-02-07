using Assessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouletteController : ControllerBase
    {
        private readonly RouletteDbContext _dbContext;

        public RouletteController(RouletteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly List<int> spinsHistory = new();
        private readonly Random random = new();

        /// <summary>
        /// Place a bet
        /// </summary>
        /// <param name="betNumber"></param>
        /// <param name="amount"></param>
        /// <returns></returns>

        [HttpPost("placebet")]
        public ActionResult PlaceBet([FromBody] BetRequestModel model)
        {
            var user = _dbContext.User.FirstOrDefault(u => u.UserName == model.UserName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            user.Balance += model.Amount;
            var bet = new Bet
            {
                UserId = user.UserId,
                BetNumber = model.BetNumber,
                Amount = model.Amount
            };
            _dbContext.Bet.Add(bet);
            _dbContext.User.Update(user);
            _dbContext.SaveChanges();

            return Ok("Bet placed successfully.");
        }

        /// <summary>
        /// Stimulates spinning the roulette wheel
        /// </summary>
        /// <returns></returns>
        [HttpGet("spin")]
        public ActionResult<int> Spin(string userName)
        {
            int spin = random.Next(0, 37);
            spinsHistory.Add(spin);
            var user = _dbContext.User.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            var SpinHistory = new SpinHistory()
            {
                UserId = user.UserId,
                Username = user.UserName,
                SpinResult = spin,
                SpinDateTime = DateTime.Now
            };
            _dbContext.SpinHistory.Add(SpinHistory);
            _dbContext.SaveChanges();

            return Ok(new { spin });
        }

        /// <summary>
        /// Returns the history of previous spins
        /// </summary>
        /// <returns></returns>
        [HttpGet("spinhistory")]
        public ActionResult<IEnumerable<int>> ShowPreviousSpins(string userName)
        {
            var spinHistory = _dbContext.SpinHistory.Where(s => s.Username == userName).ToList();
            if (spinHistory is null || !spinHistory.Any())
            {
                return BadRequest("No previous spins for user.");
            }

            return Ok(spinHistory);
        }

        /// <summary>
        /// Calculates the payout for a bet based
        /// </summary>
        /// <param name="winningNumber"></param>
        /// <param name="betAmount"></param>
        /// <returns></returns>
        [HttpGet("payout")]
        public ActionResult<int> Payout(string userName, int winningNumber, int betAmount)
        {
            var user = _dbContext.User.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            var spinHistory = _dbContext.SpinHistory.Where(s => s.Username == userName).ToList();
            if (spinHistory is null || !spinHistory.Any())
                return BadRequest("No spins yet.");

            var winningSpin = spinHistory.FirstOrDefault(sh => sh.SpinResult == winningNumber);
            if (winningSpin is null)
                return BadRequest($"Winning number: {winningNumber} has not been spun yet.");

            var payOut = betAmount * winningNumber;

            return Ok(new { payOut });
        }
    }
}