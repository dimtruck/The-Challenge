﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Domain.Repository;
using Domain.Entities;
using TheChallenge.Models;

namespace TheChallenge.Controllers
{

    public class WorkoutController : ApiController
    {
        private readonly IWorkoutRepository repository;

        public WorkoutController(IWorkoutRepository repository)
        {
            this.repository = repository;
        }

        // GET /api/workout
        public IEnumerable<DateTime> Get()
        {
            IList<DateTime> workoutDates = repository.GetWorkoutDates();
            return workoutDates;
        }

        // GET /api/workout/date
        public IList<SaveExerciseViewModel> Get(DateTime entryDate)
        {
            Workout workout = repository.GetWorkout(entryDate);
            IList<SaveExerciseViewModel> saveExerciseViewModelList = new List<SaveExerciseViewModel>();
            foreach (ExerciseEntry exerciseEntry in workout.ExerciseEntries)
            {
                saveExerciseViewModelList.Add(AutoMapper.Mapper.Map<SaveExerciseViewModel>(exerciseEntry));
            }
            return saveExerciseViewModelList;
        }

        // POST /api/workout
        public void Post(string value)
        {
        }

        // PUT /api/workout/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/workout/5
        public void Delete(int id)
        {
        }
    }
}