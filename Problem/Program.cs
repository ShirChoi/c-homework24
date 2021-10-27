using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Problem {
    public class Program {
        static string connectionString = "Server=DESKTOP-CT0BSVJ\\DEV;Database=EFC test;Trusted_Connection=True;";

        static void Main(string[] args) {
            Create(new Student() {Name = "Atan", Age = 20});
        }

        public static List<Student> GetUsers() {
            List<Student> users = new List<Student>();

            using (IDbConnection db = new SqlConnection(connectionString)) {
                users = db.Query<Student>("SELECT * FROM Students").ToList();
            }

            return users;
        }

        public static Student Get(int id) {
            Student student = null;
            using (IDbConnection db = new SqlConnection(connectionString)){
                student = db.Query<Student>("SELECT * FROM Students WHERE Id = @id", new { id }).FirstOrDefault();
            }
            return student;
        }

        public static Student Create(Student student) {
            using (IDbConnection db = new SqlConnection(connectionString)) {
                var sqlQuery = "INSERT INTO Students (Name, Age) VALUES(@Name, @Age); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = db.Query<int>(sqlQuery, student).FirstOrDefault();
                student.ID = userId;
            }

            return student;
        }
        public static void Update(Student student) {
            using (IDbConnection db = new SqlConnection(connectionString)) {
                var sqlQuery = "UPDATE Students SET Name = @Name, Age = @Age WHERE Id = @Id";

                db.Execute(sqlQuery, student);
            }
        }

        public static void Delete(int id) {
            using (IDbConnection db = new SqlConnection(connectionString)) {
                var sqlQuery = "DELETE FROM Students WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }

    public class Student {
        public int ID { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
    }
}