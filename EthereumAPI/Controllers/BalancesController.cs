using System.Threading.Tasks;
using EthereumAPI.Form;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Web3;

namespace EthereumAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BalancesController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Balance>> Get(string id)
        {
            var web3 = new Web3("http://localhost:7545");
            var balance = await web3.Eth.GetBalance.SendRequestAsync(id);
            var etherAmt = Web3.Convert.FromWei(balance.Value);

            Balance returnVal = new Balance {Address = id, Eth = etherAmt};

            return returnVal;
        }
    }
}