using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace TheChallenge.Helpers.Interface
{
    public interface IValueResolver
    {
        ResolutionResult Resolve(ResolutionResult source);
    }
}