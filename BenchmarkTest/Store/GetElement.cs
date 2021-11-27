using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Bogus;
using Store;

namespace BenchmarkTest.StoreTest
{
    [MemoryDiagnoser]
    public class GetElement
    {
        private StoreData data;
        private List<User> testUserForGet;
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
            
            this.testUserForGet = new List<User>() {testUsers.First()};
            
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


            this.testUserForGet = new List<User>() {testUsers.First()};
            
            this.testList = testUsers;
        }
        
        
        [Benchmark(Baseline = true)]
        public void TwoDictInData()
        {
            foreach (var user in testUserForGet)
            {
                this.data.GetUserByUserName(user.UserName);
            }
        }
        
        
        [Benchmark]
        public void ListTest()
        {
            foreach (var user in testUserForGet)
            {
                this.testList.Find(u => u.UserName == user.UserName);
            }
        }
    }
}