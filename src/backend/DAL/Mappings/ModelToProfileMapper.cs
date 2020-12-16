using JK.DAL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JK.DAL.Mappings
{
    public class ModelToProfileMapper : Profile
    {
        public ModelToProfileMapper()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserRecord, UserRecordDto>().ReverseMap();
        }
    }
}
