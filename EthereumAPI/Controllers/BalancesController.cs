using System.Threading.Tasks;
using EthereumAPI.Form;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace EthereumAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BalancesController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Balance>> Get(string id)
        {
            Web3 web3 = new Web3("http://localhost:7545");
            HexBigInteger balance = await web3.Eth.GetBalance.SendRequestAsync(id); // <--- maybe [var] type is better.
            decimal etherAmt = Web3.Convert.FromWei(balance.Value);

            Balance returnVal = new Balance {Address = id, Eth = etherAmt};

            return returnVal;
        }
    }
}