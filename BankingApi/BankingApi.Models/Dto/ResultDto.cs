using System.Collections.Generic;

namespace BankingApi.Models.Dto
{
    public class ResultDto<T> 
    {
        public ResultDto() : this(new List<string>())
        { }

        public ResultDto(List<string> errors)
        {
            Errors = errors;
        }

        public T Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
