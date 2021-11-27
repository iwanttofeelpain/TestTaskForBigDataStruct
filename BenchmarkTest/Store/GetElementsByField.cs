using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Bogus;
using Store;

namespace BenchmarkTest.StoreTest
{
    [MemoryDiagnoser]
    public class GetElementsByField
    {
        private StoreData data;
        private string testFieldForGet;
        private List<User> testList;
        

        [Params(50, 500, 5_000, 50_000, 500_000)]
        public int N;
        
        [GlobalSetup]
        public void GlobalSetup()
        {
        }

        [IterationSetup(Target = nameof(TwoDictInData))]
        public void IterationSetup1()
        {
            this.data = null;

            var testUserFaker = new Faker<User>()
                .RuleFor(u => u.UserName, f => $"{f.Internet.UserName()}{Guid.NewGuid()}")
                .RuleFor(u => u.FullName, f => f.Name.FullName())
                .RuleFor(u => u.City, f => f.Address.City());

            var testUsers = testUserFaker.Generate(N).ToList();
            
            this.testFieldForGet = testUsers.First().City;
            
            Dictionary<string, User> users = testUsers.ToDictionary(x => x.UserName, x => x);
            Dictionary<string, HashSet<User>> linksCitiesForUsers = testUsers
                .GroupBy(x => x.City)
                .ToDictionary(x => x.Key, x => x.ToHashSet());
            this.data = new StoreData(users, linksCitiesForUsers);
        }
        
        

        [IterationSetup(Target = nameof(ListTest))]
        public void IterationSetup2()
        {
            this.testList = null;
            
            var testUserFaker = new Faker<User>()
                .RuleFor(u => u.UserName, f => $"{f.Internet.UserName()}{Guid.NewGuid()}")
                .RuleFor(u => u.FullName, f => f.Name.FullName())
                .RuleFor(u => u.City, f => f.Address.City());

            var testUsers = testUserFaker.Generate(N).ToList();


            this.testFieldForGet = testUsers.First().City;
            
            this.testList = testUsers;
        }
        
        
        [Benchmark(Baseline = true)]
        public void TwoDictInData()
        {
            this.data.GetUsersByCity(testFieldForGet);
        }
        
        
        [Benchmark]
        public void ListTest()
        {
            this.testList.FindAll(u => u.City == testFieldForGet);
        }
    }
}