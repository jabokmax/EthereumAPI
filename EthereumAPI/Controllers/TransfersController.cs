using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Transaction = EthereumAPI.Form.Transaction;
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable SuggestVarOrType_BuiltInTypes

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
            Account account = new Account(transaction.SenderPrivateKey);
            
            Web3 web3 = new Web3(
                account, 
                "http://localhost:7545"
                );
            
            // Check balance
            HexBigInteger balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
            decimal currentEth = Web3.Convert.FromWei(balance);

            try
            {
                if (currentEth <= transaction.Amount)
                {
                    return Conflict();
                }

                Task<TransactionReceipt> newTransaction = web3.Eth.GetEtherTransferService()
                    .TransferEtherAndWaitForReceiptAsync(
                        transaction.RecieverAddress,
                        transaction.Amount
                    );
                
                Console.WriteLine(newTransaction.Result.ToString());

                Transaction result = new Transaction
                {
                    SenderAddress      = account.Address,
                    RecieverAddress    = transaction.RecieverAddress,
                    Amount             = transaction.Amount
                };

                return result;

            }
            catch (Exception e)
            {
                ObjectResult result = StatusCode(StatusCodes.Status500InternalServerError, e);
                return result;
            }


        }
        
        
    }
}