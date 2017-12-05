using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace SimchaDonors.Data
{
    public class SimchaManager
    {
        private string _connectionstring;
        public SimchaManager(string Connectionstring)
        {
            _connectionstring = Connectionstring;
        }
        public IEnumerable<Simcha> GetAllSimchas()
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Simchas";
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Simcha> simchas = new List<Simcha>();
            while (reader.Read())
            {
                Simcha s = new Simcha();

                s.Id = (int)reader["Id"];
                s.Name = (string)reader["Name"];
                s.Date = (DateTime)reader["Date"];
                s.Count = GetContributorCount(s.Id);
                s.Balance = GetBalanceForSimcha(s.Id);
                simchas.Add(s);
            }
            return simchas;
        }
        public IEnumerable<Contributor> GetAllContributors()
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Contributors";
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Contributor> contributors = new List<Contributor>();
            while (reader.Read())
            {
                Contributor c = new Contributor
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    CellNumber = (string)reader["CellNumber"],
                    AllwaysInclude = (bool)reader["AllwaysInclude"],

                };
                Decimal balance = GetBalance(c.Id);
                c.Balance = balance;
                contributors.Add(c);
            }
            return contributors;
        }
        public IEnumerable<Contributor> GetAllContributors(string search)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Contributors Where LastName LIKE %search%"; ;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Contributor> contributors = new List<Contributor>();
            while (reader.Read())
            {
                Contributor c = new Contributor();

                c.Id = (int)reader["Id"];
                c.FirstName = (string)reader["FirstName"];
                c.LastName = (string)reader["LastName"];
                c.CellNumber = (string)reader["CellNumber"];
                c.AllwaysInclude = (bool)reader["AllwaysInclude"];
                c.Balance = GetBalance(c.Id);


                contributors.Add(c);
            }
            return contributors;
        }
        public IEnumerable<Contribution> GetAllContributions()
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Contributions";
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Contribution> contributions = new List<Contribution>();
            while (reader.Read())
            {
                Contribution contribution = new Contribution
                {
                    id = (int)reader["id"],
                    Contributorid = (int)reader["ContributorId"],
                    Amount = (Decimal)reader["Amount"],
                    Date = (DateTime)reader["Date"],
                };
                contributions.Add(contribution);
            }
            return contributions;
        }
        public void AddContributor(Contributor c)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Contributors(FirstName,LastName,CellNumber,AllwaysInclude)
                                    VALUES(@firstname,@lastname,@cellnum,@allwaysinclude); SELECT @@Identity";
            command.Parameters.AddWithValue("@firstname", c.FirstName);
            command.Parameters.AddWithValue("@lastname", c.LastName);
            command.Parameters.AddWithValue("@cellnum", c.CellNumber);
            command.Parameters.AddWithValue("@AllwaysInclude", c.AllwaysInclude);
            connection.Open();
            c.Id = (int)(decimal)command.ExecuteScalar();
        }
        public void AddContribution(Contribution c)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Contributions(Contributorid,Amount,Date)
                                    VALUES(@contributorid,@amount,@date); SELECT @@Identity";
            command.Parameters.AddWithValue("@contributorid", c.Contributorid);
            command.Parameters.AddWithValue("@amount", c.Amount);
            command.Parameters.AddWithValue("@date", c.Date);
            connection.Open();
            c.id = (int)(decimal)command.ExecuteScalar();
        }
        public void AddSimcha(Simcha s)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO simchas(Name,Date)
                                    VALUES(@name,@date); SELECT @@Identity";
            command.Parameters.AddWithValue("@name", s.Name);
            command.Parameters.AddWithValue("@date", s.Date);
            connection.Open();

            s.Id = (int)(decimal)command.ExecuteScalar();
        }
        public decimal GetBalance(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "Select Amount from Contributions where contributorid=@id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            Decimal contribution = 0;
            while (reader.Read())
            {
                contribution += (Decimal)reader["Amount"];
            }

            command.CommandText = "Select amount from SimchaContribution where contributorid=@id";
            command.Parameters.AddWithValue("@id", id);
            Decimal deduction = 0;
            while (reader.Read())
            {
                deduction += (Decimal)reader["Amount"];
            }
            Decimal Total = (contribution - deduction);
            return Total;

        }
        public int GetContributorCount(int simchaid)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(simchaid) as Count FROM SimchaContribution WHERE Simchaid=@simchaid";
            command.Parameters.AddWithValue("@simchaid", simchaid);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int count = (int)(reader)["Count"];
            return count;
        }
        public int AllContributorsCount()
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(FirstName) As Count FROM Contributors";
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int Total = (int)(reader)["Count"];
            return Total;
        }
        public Decimal GetBalanceForSimcha(int simchaid)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT Amount from SimchaContribution Where simchaid=@simchaid";
            command.Parameters.AddWithValue("@simchaid", simchaid);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            Decimal Balance = 0;
            while (reader.Read())
            {
                Balance += (decimal)reader["Amount"];

            }
            return Balance;

        }
        public Contributor GetContributor(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * from Contributors where id=@id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Contributor c = new Contributor
            {
                Id = (int)reader["Id"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                CellNumber = (string)reader["CellNumber"],
                AllwaysInclude = (bool)reader["AllwaysInclude"],

            };
            Decimal balance = GetBalance(c.Id);
            c.Balance = balance;
            return c;
        }
        public IEnumerable<Contribution> GetContributionsForid(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT Amount, Date FROM Contributions Where contributorid = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            List<Contribution> contribution = new List<Contribution>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Contribution c = new Contribution
                {
                    Amount = (Decimal)reader["Amount"],
                    Date = (DateTime)reader["Date"],
                };
                contribution.Add(c);
            }
            command.CommandText = @"select sc.Amount, s.Date from SimchaContributions sc
                                    Join Simchas s 
                                    on sc.simchaid=s.id
                                    where sc.contributorid=@id";
            while (reader.Read())
            {
                Contribution c2 = new Contribution
                {
                    Amount = (Decimal)reader["Amount"],
                    Date = (DateTime)reader["Date"],
                };
                contribution.Add(c2);
            }

            return contribution;

        }
        public void DonateToSimcha(DonateToSimcha simcha)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE from SimchaContributions where @simchaid = simchaid" + "INSERT INTO SimchaContribution VALUES(@simchaid,@contributorid, @amount)";
            command.Parameters.AddWithValue("@simchaid", simcha.Simchaid);
            command.Parameters.AddWithValue("@contributorid", simcha.Contributorid);
            command.Parameters.AddWithValue("@amount", simcha.Amount);
            connection.Open();
            command.ExecuteNonQuery();


        }
        public IEnumerable<DonateToSimcha> GetAllSimchaDonations(int simchaid)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM SimchaContribution where simchaid = @simchaid";
            command.Parameters.AddWithValue("@simchaid", simchaid);
            connection.Open();
            List<DonateToSimcha> donors = new List<DonateToSimcha>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                DonateToSimcha simcha = new DonateToSimcha
                {
                    Simchaid = (int)reader["simchaid"],
                    Contributorid = (int)reader["contributorid"],
                    Amount = (Decimal)reader["Amount"]

                };
                donors.Add(simcha);

            }
            return donors;


        }
    }

}

