using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using TheChallenge.Controllers;
using Domain.Repository;
using Domain.Entities;
using TheChallenge.Models;
using Moq;
using TheChallenge.Helpers;

namespace Test
{
    class GoalTest : nspec
    {
        private GoalController controller;
        private Moq.Mock<IProfileRepository> profileRepositoryMock;
        private IList<ContestEventGoal> goals = new List<ContestEventGoal>(){
            new ContestEventGoal(){
                Id = 1
            },
            new ContestEventGoal(){
                Id = 2
            }
        };
        private IList<ContestEventGoalViewModel> goalsViewModel = new List<ContestEventGoalViewModel>(){
            new ContestEventGoalViewModel(),
            new ContestEventGoalViewModel()
        };


        private void GoalSetup()
        {
            AutoMapper.Mapper.CreateMap<ContestEventGoal, ContestEventGoalViewModel>()
                .ForMember(dest => dest.Result, opt => opt.ResolveUsing<GoalDataResolver>());
            profileRepositoryMock = new Moq.Mock<IProfileRepository>();
            profileRepositoryMock.Setup(t => t.RetrieveGoals()).Returns(goals);
            controller = new GoalController(profileRepositoryMock.Object);
        }


        void describe_Goals()
        {
            context["When retrieving goals"] = () =>
                {
                    beforeEach = () => GoalSetup();
                    it["Should retrieve goals"] = () =>
                    {
                        controller.Get().Count.should_be(goalsViewModel.Count);
                    };
                    it["Should call profile repository and return goal objects"] = () =>
                    {
                        controller.Get();
                        profileRepositoryMock.Verify<IList<ContestEventGoal>>(t => t.RetrieveGoals(), Times.Once());
                    };
                };
        }
    }
}
