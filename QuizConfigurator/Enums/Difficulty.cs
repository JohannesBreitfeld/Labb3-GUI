using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuizConfigurator.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<Difficulty>))]
    enum Difficulty
    {
        Easy,
        Medium,
        Hard 
    }

}
