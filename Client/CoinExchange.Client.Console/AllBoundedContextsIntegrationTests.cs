/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* Coin Exchange is a high performance exchange system specialized for
* Crypto currency trading. It has different general purpose uses such as
* independent deposit and withdrawal channels for Bitcoin and Litecoin,
* but can also act as a standalone exchange that can be used with
* different asset classes.
* Coin Exchange uses state of the art technologies such as ASP.NET REST API,
* AngularJS and NUnit. It also uses design patterns for complex event
* processing and handling of thousands of transactions per second, such as
* Domain Driven Designing, Disruptor Pattern and CQRS With Event Sourcing.
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinExchange.Client.Tests;
using CoinExchange.Common.Tests;
using Spring.Context;
using Spring.Context.Support;

namespace CoinExchange.Client.Console
{
    /// <summary>
    /// Contains tests that test all the bounded contexts running mutually
    /// </summary>
    public class AllBoundedContextsIntegrationTests
    {
        private AccessControl _accessControl;
        private IdentityAccessClient _identityAccessClient;
        private IApplicationContext _applicationContext;
        private DatabaseUtility _databaseUtility;

        private string _baseUrlCloud = "http://crypgo.cloudapp.net/test/v1";
        private string _baseUrlLocalhost = "http://localhost:51780/v1";
        private FundsClient _fundsClient;
        private string _username = "jonsnow";
        private string _password = "pa$$word";
        private string _email = "waqas.shah.syed@gmail.com";
        private string _baseCurrency = "BTC";
        private string _quoteCurrency = "LTC";

        public AllBoundedContextsIntegrationTests()
        {
            _fundsClient = new FundsClient(_baseUrlLocalhost);
            _identityAccessClient = new IdentityAccessClient(_baseUrlLocalhost);

            _accessControl = new AccessControl(_identityAccessClient, _password, _username);
        }

        public void Initialization()
        {
            //ClearDatabase();
            UserLogin();

            _fundsClient.key = _identityAccessClient.key;
            _fundsClient.secretkey = _identityAccessClient.secretkey;

            //SendOrders();
            ApplyForTier1();
            VerifyTier1();

            ApplyForTier2();
            VerifyTier2();

            //ApplyForTier3();
            //VerifyTier3();

            //MakeDeposit();
            GetLimits();
            //TradeExecuted();
            //CreateDepositAddress();
            //GetDepositAddress();
            //GetRecentDeposits();
            //AddWithdrawAddress();
            GetWithdrawAddress();
            System.Console.ReadLine();
        }

        private void ClearDatabase()
        {
            //_applicationContext = ContextRegistry.GetContext();
            var connection = ConfigurationManager.ConnectionStrings["MySql"].ToString();
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();
            _databaseUtility.Populate();
            System.Console.WriteLine("Database cleared and initialized");
        }

        private void UserLogin()
        {
            System.Console.WriteLine("User sign up start");
            _accessControl.CreateAndActivateUser(_username, _password, _email);
            System.Console.WriteLine("User sign up end");

            System.Console.WriteLine("User log in start");
            _accessControl.Login(_username, _password);
            System.Console.WriteLine("User log in end");
        }

        private void MakeDeposit()
        {
            System.Console.WriteLine("Deposit start");
            _fundsClient.MakeDeposit(_baseCurrency, 2000, true, "");
            _fundsClient.MakeDeposit(_quoteCurrency, 2000, true, "123");
            System.Console.WriteLine("Deposit end");
        }

        private void CreateDepositAddress()
        {
            System.Console.WriteLine("Create Deposit Address start");
            System.Console.WriteLine(_fundsClient.CreateDepositAddress(_quoteCurrency));
            System.Console.WriteLine("Create Deposit Address  end");
        }



        private void ApplyForTier1()
        {
            System.Console.WriteLine("Tier 1 apply start");
            System.Console.WriteLine(_identityAccessClient.ApplyForTierLevel1("Rod Holt", DateTime.Now.AddYears(57).ToShortDateString(),
                "+1244322222"));

            System.Console.WriteLine("Tier 1 Apply end");
        }

        private void ApplyForTier2()
        {
            System.Console.WriteLine("Tier 2 apply start");
            System.Console.WriteLine(_identityAccessClient.ApplyForTierLevel2("H 757", "H 675", "H 980", "Punjab", "Rwp", "46000"));

            System.Console.WriteLine("Tier 2 Apply end");
        }

        private void ApplyForTier3()
        {
            System.Console.WriteLine("Tier 3 apply start");
            System.Console.WriteLine(_identityAccessClient.ApplyForTierLevel3("H 757", "H 675", ".png", "shenfenzheng"));

            System.Console.WriteLine("Tier 3 Apply end");
        }

        private void VerifyTier1()
        {
            System.Console.WriteLine("Tier 1 Verification Start");
            System.Console.WriteLine(_identityAccessClient.VerifyTierLevel(_identityAccessClient.key, "Tier 1"));

            System.Console.WriteLine("Tier 1 Verification End");
        }

        private void VerifyTier2()
        {
            System.Console.WriteLine("Tier 2 Verification Start");
            System.Console.WriteLine(_identityAccessClient.VerifyTierLevel(_identityAccessClient.key, "Tier 2"));

            System.Console.WriteLine("Tier 2 Verification End");
        }

        private void VerifyTier3()
        {
            System.Console.WriteLine("Tier 3 Verification Start");
            System.Console.WriteLine(_identityAccessClient.VerifyTierLevel(_identityAccessClient.key, "Tier 3"));

            System.Console.WriteLine("Tier 3 Verification End");
        }

        private void GetLimits()
        {
            System.Console.WriteLine("Get Limits start");
            System.Console.WriteLine(_fundsClient.GetDepositLimits(_baseCurrency));
            System.Console.WriteLine(_fundsClient.GetWithdrawalLimits(_baseCurrency));
            System.Console.WriteLine(_fundsClient.GetDepositTierLimits());
            System.Console.WriteLine("Get Limits end");
        }

        private void AddWithdrawAddress()
        {
            System.Console.WriteLine("Add Withdraw Address start");
            System.Console.WriteLine(_fundsClient.AddWithdrawAddress(_quoteCurrency, "mgTbDyNGwJeewjdXmU9cRQe8WDauVqn4WK", "testnet"));
            System.Console.WriteLine("Add Withdraw Address end");
        }

        private void GetWithdrawAddress()
        {
            System.Console.WriteLine("Get Withdraw Address start");
            System.Console.WriteLine(_fundsClient.GetWithdrawAddress(_quoteCurrency));
            System.Console.WriteLine("Get Withdraw Address end");
        }

        private void GetDepositAddress()
        {
            System.Console.WriteLine("Get Deposit Address start");
            System.Console.WriteLine(_fundsClient.GetDepositAddress(_quoteCurrency));
            System.Console.WriteLine("Get Deposit Address end");
        }

        private void GetRecentDeposits()
        {
            System.Console.WriteLine("Get Recent Deposit start");
            System.Console.WriteLine(_fundsClient.GetRecentDeposits(_quoteCurrency));
            System.Console.WriteLine("Get Recent Deposit end");
        }

        private void SendOrders()
        {
            System.Console.WriteLine("Send orders start");
            System.Console.WriteLine(_identityAccessClient.CreateOrder(_baseCurrency + _quoteCurrency, "limit", "buy", 2, 250));
            System.Console.WriteLine(_identityAccessClient.CreateOrder(_baseCurrency + _quoteCurrency, "limit", "sell", 2, 260));
            System.Console.WriteLine(_identityAccessClient.CreateOrder(_baseCurrency + _quoteCurrency, "limit", "buy", 2, 251));
            System.Console.WriteLine(_identityAccessClient.CreateOrder(_baseCurrency + _quoteCurrency, "limit", "sell", 2, 262));
            System.Console.WriteLine("Send orders end");
        }

        private void TradeExecuted()
        {
            System.Console.WriteLine("Trade execution start");
            System.Console.WriteLine(_identityAccessClient.CreateOrder(_baseCurrency + _quoteCurrency, "limit", "buy", 2, 260));
            System.Console.WriteLine("Trade execution end");
        }
    }
}
