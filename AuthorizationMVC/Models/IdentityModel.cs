using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AuthorizationMVC.Models
{
    public static class ApplicationDbContext
    {
        public static Users CheckLoginInDb(string Email, string Password)
        {
            Users user = new Users();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryCheckLoginInDb(Email, Password), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.IdUser = Convert.ToInt32(reader[0]);
                            user.Name = reader[1].ToString();
                            user.Email = reader[2].ToString();
                            user.Password = reader[3].ToString();
                        }
                    }
                }
            }
            return user;
        }

        private static string GetQueryCheckLoginInDb(string Email, string password)
        {
            string result = "";
            result = string.Format(@"select 
                                        u.IdUser
                                        ,u.Name
                                        ,u.Email
                                        ,u.Password 
                                    from 
                                        dbo.Users as u
                                    where 
                                        Email = '{0}' and Password = '{1}'", Email, password);
            return result;
        }

        public static string GetRoleForUser(string Email)
        {
            string role = "";
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryRoleForUser(Email), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            role = reader[0].ToString();
                        }
                    }
                }
            }
            return role;
        }

        private static string GetQueryRoleForUser(string Email)
        {
            string query = "";
            query = string.Format(@"select 
                                        r.Role 
                                    from 
                                        dbo.Roles as r 
                                        inner join dbo.UserRole as ur on r.IdRole = ur.IdRole 
                                        inner join dbo.Users as u on u.IdUser = ur.IdUser
                                    where 
                                        u.Email = '{0}'", Email);
            return query;
        }

        public static bool CheckEmailInDb(string Email)
        {
            Users user = new Users();
            string con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryCheckEmailInDb(Email), conn))
                {                    
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.IdUser = Convert.ToInt32(reader[0]);
                            user.Name = reader[1].ToString();
                            user.Email = reader[2].ToString();
                            user.Password = reader[3].ToString();
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static string GetQueryCheckEmailInDb(string Email)
        {
            string result = "";
            result = string.Format("select IdUser, Name, Email, Password from dbo.Users where Email = '{0}'", Email);
            return result;
        }

        public static bool RegisterUserInDb(RegisterModel model)
        {
            bool flag = false;
            string con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(con))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryRegisterUserInDb(model), conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        private static string GetQueryRegisterUserInDb(RegisterModel model)
        {
            string result = "";
            result = string.Format(@"insert into dbo.Users (Name, Email, Password) values ('{0}', '{1}', '{2}')
                                    insert into dbo.UserRole (IdUser, IdRole) values ((select IdUser from dbo.Users where Email = '{1}'), '{3}')", model.Name, model.Email, model.Password, model.Role);
            return result;
        }

        public static List<Roles> GetRolesListFromDb()
        {
            List<Roles> roles = new List<Roles>();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryGetRolesListFromDb(), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Roles r = new Roles();
                            r.IdRole = Convert.ToInt32(reader[0]);
                            r.Role = reader[1].ToString();
                            roles.Add(r);
                        }
                    }
                }
            }
            return roles;
        }

        private static string GetQueryGetRolesListFromDb()
        {
            string query = "";
            query = "select IdRole, Role from dbo.Roles";
            return query;
        }

        public static List<Departments> GetDepartmentListFromDb()
        {
            List<Departments> dep = new List<Departments>();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryGetDepartmentListFromDb(), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Departments department = new Departments();
                            department.IdDepartment = Convert.ToInt32(reader[0]);
                            department.Name = reader[1].ToString();
                            dep.Add(department);
                        }
                    }
                }
            }
            return dep;
        }

        private static string GetQueryGetDepartmentListFromDb()
        {
            string query = "";
            query = "select IdDepartment, Name from dbo.Departments";
            return query;
        }

        public static List<Positions> GetPositionsDepartmentFromDb(string IdDepartment)
        {
            List<Positions> positions = new List<Positions>();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryGetPositionsDepartmentFromDb(IdDepartment), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Positions pos = new Positions();
                            pos.IdPosition = Convert.ToInt32(reader[0]);
                            pos.Name = reader[1].ToString();
                            pos.IdDepartment = Convert.ToInt32(reader[2]);
                            positions.Add(pos);
                        }
                    }
                }
            }
            return positions;
        }

        private static string GetQueryGetPositionsDepartmentFromDb(string IdDepartment)
        {
            string query = "";
            query = string.Format("select IdPosition, Name, IdDepartment from dbo.Positions where IdDepartment={0}", IdDepartment);
            return query;
        }

        public static void DbInitialize()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            bool emptyDatabase = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryAnyUserInDb(), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (string.IsNullOrEmpty(reader[0].ToString()))
                            {
                                emptyDatabase = true;
                            }
                        }
                    }
                }
                if (emptyDatabase)
                {
                    using (SqlCommand cmd = new SqlCommand(GetQueryDbInitialize(), conn))
                    {
                        conn.Open();
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch
                        { }
                    }
                }
            }
        }

        private static string GetQueryAnyUserInDb()
        {
            string query = "";
            query = "select IdUser from dbo.Users";
            return query;
        }

        private static string GetQueryDbInitialize()
        {
            string query = "";
            query = @"insert into dbo.Roles(Role) values('admin')
                      insert into dbo.Users (Name, Email, Password) values ('admin', 'administrator@admin.com', 'admin')
                      insert into dbo.UserRole (IdUser, IdRole) values ((select IdUser from dbo.Users where Email = 'administrator@admin.com'), (select IdRole from dbo.Roles where Role = 'admin'))
                      insert into dbo.Departments (Name) values('President')
                      insert into dbo.Positions (Name, IdDepartment) values ('President', (select IdDepartment from dbo.Departments where Name = 'President'))
                      insert into dbo.UserData (IdUser, Sex, Age, Earning, IdPosition) 
                      values((select IdUser from dbo.Users where Email='administrator@admin.com'), 'M', 40, 50000, (select IdPosition from dbo.Positions where Name = 'President'))";                      
            return query;
        }

        public static bool CheckDepartmentInDb(string DepartmentName)
        {
            bool inDb = false;
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryCheckDepartmentInDb(DepartmentName), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            if(!string.IsNullOrEmpty(reader[0].ToString()))
                            {
                                inDb = true;
                            }
                        }
                    }
                    
                }
            }
            return inDb;
        }

        private static string GetQueryCheckDepartmentInDb(string DepartmentName)
        {
            string query = "";
            query = string.Format("select * from dbo.Departments where Name = '{0}'", DepartmentName);
            return query;
        }

        public static bool SetDepartmentToDb(string DepartmentName)
        {
            bool add = false;
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQuerySetDepartmentToDb(DepartmentName), conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        add = true;
                    }
                    catch(Exception ex)
                    {
                        add = false;
                    }
                }
            }
            return add;
        }

        private static string GetQuerySetDepartmentToDb(string DepartmentName)
        {
            string query = "";
            query = string.Format("insert into dbo.Departments(Name) values ('{0}')", DepartmentName);
            return query;
        }

        public static bool CheckPositionInDb(PositionModel model)
        {
            bool inDb = false;
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryCheckPositionInDb(model), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            if (!string.IsNullOrEmpty(reader[0].ToString()))
                            {
                                inDb = true;
                            }
                        }
                    }
                }
            }
            return inDb;
        }

        private static string GetQueryCheckPositionInDb(PositionModel model)
        {
            string query = "";
            query = string.Format("select IdPosition from dbo.Positions where IdDepartment = {0} and Name = '{1}'", model.IdDepartment, model.Name);
            return query;
        }

        public static bool SetPositionToDb(PositionModel model)
        {
            bool add = false;
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQuerySetPositionToDb(model), conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        add = true;
                    }
                    catch (Exception ex)
                    {
                        add = false;
                    }
                }
            }
            return add;
        }

        private static string GetQuerySetPositionToDb(PositionModel model)
        {
            string query = "";
            query = string.Format("insert into dbo.Positions(Name, IdDepartment) values ('{0}', {1})", model.Name, model.IdDepartment);
            return query;
        }

        public static UpdateModel GetInfoUserByEmail(string Email)
        {
            UpdateModel model = new UpdateModel();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryGetInfoUserByEmail(Email), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Name = reader[0].ToString();
                            model.Password = reader[1].ToString();
                            try
                            {
                                model.Age = Convert.ToInt32(reader[2]);
                                model.Earning = Convert.ToDecimal(reader[3]);
                            }
                            catch(Exception ex)
                            { }
                        }
                    }
                }
            }
            return model;
        }

        private static string GetQueryGetInfoUserByEmail(string Email)
        {
            string query = "";
            query = string.Format(@"select 
                        u.Name
                        ,u.Password 
                        ,ud.Age
                        ,ud.Earning
                      from 
                        dbo.Users as u
                        left join dbo.UserData as ud on u.IdUser = ud.IdUser
                      where
                        u.Email = '{0}'", Email);
            return query;
        }

        public static bool UpdateUserInfoInDb(UpdateModel model, string email)
        {
            bool updated = false;
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryUpdateUserInfoInDb(model, email), conn))
                {
                    conn.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        updated = true;
                    }
                    catch (Exception ex)
                    {
                        updated = false;
                    }
                }
            }
            return updated;
        }

        private static string GetQueryUpdateUserInfoInDb(UpdateModel model, string email)
        {
            string query = string.Format(@"update dbo.Users set Name = '{0}', Password = '{1}' where Email = '{2}'
                                           update dbo.UserData set Age = '{3}', Earning = '{4}' where IdUser in (select IdUser from dbo.Users where Email = '{2}')", model.Name, model.Password, email, model.Age, model.Earning);
            return query;
        }

        public static List<string> ReportGetEmplyeesDepartment(string email)
        {
            bool president = false;
            string position = "";
            List<string> employees = new List<string>();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryPositionEmployee(email), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            position = reader[0].ToString();
                            if (position == "President")
                            {
                                president = true;
                            }
                        }
                    }
                }
                if (!president)
                {
                    using (SqlCommand cmd = new SqlCommand(GetQueryReportGetEmplyeesDepartment(email), conn))
                    {                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(reader[0].ToString());
                            }
                        }
                    }
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand(GetQueryReportGetAllEmplyees(), conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(reader[0].ToString());
                            }
                        }
                    }
                }
            }
            return employees;
        }

        private static string GetQueryPositionEmployee(string email)
        {
            string query = "";
            query = string.Format(@"select 
                        p.Name 
                    from 
                        dbo.Users as u 
                        inner join dbo.UserData as ud on u.IdUser = ud.IdUser 
                        inner join dbo.Positions as p on p.IdPosition = ud.IdPosition
                    where
                        u.Email = '{0}'", email);
            return query;
        }

        private static string GetQueryReportGetEmplyeesDepartment(string email)
        {
            string query = "";
            query = string.Format(@"
                    select
                        u.Name
                    from
                        dbo.Departments as d
                        inner join dbo.Positions as p on d.IdDepartment = p.IdDepartment
                        inner join dbo.UserData as ud on p.IdPosition = ud.IdPosition
                        inner join dbo.Users as u on ud.IdUser = u.IdUser
                    where
                        d.IdDepartment in (select 
                                                d.IdDepartment
                                            from 
                                                dbo.Users as u 
                                                inner join dbo.UserData as ud on u.IdUser = ud.IdUser 
                                                inner join dbo.Positions as p on ud.IdPosition = p.IdPosition
                                                inner join dbo.Departments as d on d.IdDepartment = p.IdDepartment
                                            where
                                                u.Email = '{0}')
                    ", email);
            return query;
        }

        private static string GetQueryReportGetAllEmplyees()
        {
            string query = "";
            query = "select Name from dbo.Users";
            return query;
        }        

        public static List<String> ReportGetPositionsDepartment(string email)
        {
            bool president = false;
            string position = "";
            List<string> positions = new List<string>();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GetQueryPositionEmployee(email), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            position = reader[0].ToString();
                            if (position == "President")
                            {
                                president = true;
                            }
                        }
                    }
                }
                if (!president)
                {
                    using (SqlCommand cmd = new SqlCommand(GetQueryReportGetPositionsDepartment(email), conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                positions.Add(reader[0].ToString());
                            }
                        }
                    }
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand(GetQueryGetAllPositions(), conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                positions.Add(reader[0].ToString());
                            }
                        }
                    }
                }
            }
            return positions;
        }

        private static string GetQueryReportGetPositionsDepartment(string email)
        {
            string query = "";
            query = string.Format(@"
                    select
                        p.Name
                    from
                        dbo.Departments as d
                        inner join dbo.Positions as p on d.IdDepartment = p.IdDepartment
                    where
                        d.IdDepartment in (select 
                                                d.IdDepartment
                                            from 
                                                dbo.Users as u 
                                                inner join dbo.UserData as ud on u.IdUser = ud.IdUser 
                                                inner join dbo.Positions as p on ud.IdPosition = p.IdPosition
                                                inner join dbo.Departments as d on d.IdDepartment = p.IdDepartment
                                            where
                                                u.Email = '{0}')
                    ", email);
            return query;
        }

        private static string GetQueryGetAllPositions()
        {
            string query = "";
            query = "select Name from dbo.Positions";
            return query;
        }
    }

    public class Users
    {
        public int IdUser { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }

    public class Roles
    {
        public int IdRole { get; set; }

        public string Role { get; set; }
    }

    public class Departments
    {
        public int IdDepartment { get; set; }

        public string Name { get; set; }
    }

    public class Positions
    {
        public int IdPosition { get; set; }

        public string Name { get; set; }

        public int IdDepartment { get; set; }
    }

    public class UserData
    {
        public int Id { get; set; }

        public int IdUser { get; set; }

        public char Sex { get; set; }

        public int Age { get; set; }

        public decimal Earning { get; set; }

        public int IdPosition { get; set; }
    }
}