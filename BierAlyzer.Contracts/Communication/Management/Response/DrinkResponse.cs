using System.Collections.Generic;
using BierAlyzer.Contracts.Dto;
using BierAlyzer.Contracts.Interface.Communication;
using BierAlyzer.Contracts.Model;

namespace BierAlyzer.Contracts.Communication.Management.Response
{
    public class DrinkResponse : IApiResponseParameter
    {
        public List<DrinkDto> Drinks { get; set; }
        public RequestResult Result { get; set; }

        public DrinkResponse()
        {
            Drinks = new List<DrinkDto>();
            Result = new RequestResult();
        }
    }
}
