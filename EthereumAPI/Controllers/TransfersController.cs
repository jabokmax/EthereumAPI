using System;
using System.Threading.Tasks;
using EthereumAPI.Form;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace EthereumAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<Transaction>> Post(Transaction transaction)
        {
            
            Console.WriteLine($"Transferring From {transaction.SenderPrivateKey} to {transaction.RecieverAddress} value {transaction.Amount}");

            // Create Instance 
            var account = new Account(transaction.SenderPrivateKey);
            
            var web3 = new Web3(
                account, 
                "http://localhost:7545"
                );
            
            // Check balance
            var balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
            var currentEth = Web3.Convert.FromWei(balance);

            try
            {
                if (currentEth <= transaction.Amount)
                {
                    return Conflict();
                }
                else
                {
                    
                    var newTransaction = web3.Eth.GetEtherTransferService()
                        .TransferEtherAndWaitForReceiptAsync(
                            transaction.RecieverAddress,
                            transaction.Amount
                            );

                    var result = new Transaction
                    {
                        SenderAddress      = account.Address,
                        RecieverAddress    = transaction.RecieverAddress,
                        Amount             = transaction.Amount
                    };

                    return result;
                }

            }
            catch (Exception e)
            {
                var result = StatusCode(StatusCodes.Status500InternalServerError, e);
                return result;
            }


        }
        
        
    }
}