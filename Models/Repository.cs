using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;

namespace Project01.Models
{
    public interface IRepository
    {
        IEnumerable<Person> People { get; }

        IEnumerable<Team> Teams { get; }

        Person AddPerson(Person person);

        Person UpdatePerson(Person person);

        Team AddTeam(Team newTeam);
    }

    public class DbRepository : IRepository
    {
        private IDbConnection _connection;

        public DbRepository(string connectionString) => _connection = new SqliteConnection(connectionString);

        public IEnumerable<Person> People 
            => _connection.Query<Person>(
                sql: "select Id, FirstName, LastName, Gender, BirthDate, TeamId, " +
                    "(select Name from Teams where Teams.Id = Persons.TeamId) as TeamName, Country from Persons",
                types: new [] { typeof(int), typeof(string), typeof(string), typeof(string), typeof(string),
                    typeof(int), typeof(string), typeof(string) },
                map: values =>
                    new Person { Id = Convert.ToInt32(values[0]), FirstName = (string)values[1], LastName = (string)values[2],
                        Gender = (string)values[3], BirthDate = (string)values[4],
                        Team = new Team { Id = Convert.ToInt32(values[5]), Name = (string)values[6] }, Country = (string)values[7]
                    },
                splitOn: "Id,FirstName,LastName,Gender,BirthDate,TeamId,TeamName,Country"
            );

        public IEnumerable<Team> Teams
            => _connection.Query<Team>("select Id, Name from Teams");

        public Person AddPerson(Person newPerson) {
            if (!Teams.Select(t => t.Name).Contains(newPerson.Team.Name))
                newPerson.Team = AddTeam(newPerson.Team);
            else
                newPerson.Team.Id = _connection.QueryFirst<int>($"select Id from Teams where Teams.Name = '{newPerson.Team.Name}'");
            newPerson.Id = _connection.QueryFirst<int>("select count(*) from Persons") + 1;
            _connection.Query<Person>("insert into Persons values(" +
                $"'{newPerson.Id}'," +
                $"'{newPerson.FirstName}'," +
                $"'{newPerson.LastName}'," +
                $"'{newPerson.Gender}'," +
                $"'{newPerson.BirthDate}'," +
                $"'{newPerson.Team.Id}'," +
                $"'{newPerson.Country}')"
            );

            return newPerson;
        }

        public Person UpdatePerson(Person person) {
            if (!Teams.Select(t => t.Name).Contains(person.Team.Name))
                person.Team = AddTeam(person.Team);
            else
                person.Team.Id = _connection.QueryFirst<int>($"select Id from Teams where Teams.Name = '{person.Team.Name}'");
            _connection.Query<Person>("update Persons set " + 
                $"Id = '{person.Id}', " +
                $"FirstName = '{person.FirstName}', " +
                $"LastName = '{person.LastName}', " +
                $"Gender = '{person.Gender}', " +
                $"BirthDate = '{person.BirthDate}', " +
                $"TeamId = '{person.Team.Id}', " +
                $"Country = '{person.Country}' " +
                $"where Id = {person.Id}"
            );

            return person;
        }

        public Team AddTeam(Team newTeam) {
            newTeam.Id = Teams.Count() + 1;
            _connection.Query<Team>($"insert into Teams values('{newTeam.Id}', '{newTeam.Name}')");

            return newTeam;
        }
    }
}