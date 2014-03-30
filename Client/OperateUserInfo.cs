using System;
using System.Data.Common;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

namespace Client
{
    partial class OperateUserInfo
    {
        public OperateUserInfo()
        {
        }
        // 验证用户记录
        public int ValidateUserInfo(UserInfo userInfo)
        {
            userInfo.Password = EncryptPassword(userInfo.Password);
            int i = 0;
            if (File.Exists("User.config"))
            {
                using (Stream stream = new FileStream("User.config", FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    XmlSerializer xmlSerialize = new XmlSerializer(typeof(List<UserInfo>));
                    List<UserInfo> userList= (List<UserInfo>)xmlSerialize.Deserialize(stream);
                    // LINQ 查询
                    i = (from user in userList
                         where user.Name.ToUpper() == userInfo.Name.ToUpper() && user.Password == userInfo.Password
                         select user).Count();
                    
                }
            }
            return i;
        }

        // 增加新用户
        public bool AddUserInfo(UserInfo userInfo)
        {
            userInfo.Password = EncryptPassword(userInfo.Password);
            List<UserInfo> userList;
            if (File.Exists("User.config"))
            {
                XmlSerializer xmlSerialize = new XmlSerializer(typeof(List<UserInfo>));
                using (Stream stream = new FileStream("User.config", FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    try
                    {
                        userList = (List<UserInfo>)xmlSerialize.Deserialize(stream);
                        userList.Add(new UserInfo { Name = userInfo.Name, Password = userInfo.Password, Email = userInfo.Email });
                    }
                    catch
                    {
                        return false;   
                    }
                }
                using (Stream stream = new FileStream("User.config", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    try
                    {
                        xmlSerialize.Serialize(stream,userList);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            else
            {
                using (Stream stream = new FileStream("User.config", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    userList = new List<UserInfo> 
                    {
                        new UserInfo { Name = userInfo.Name, Password = userInfo.Password, Email = userInfo.Email }
                    };
                    try
                    {
                        XmlSerializer xmlSerialize = new XmlSerializer(typeof(List<UserInfo>));
                        xmlSerialize.Serialize(stream, userList);
                    }
                    catch (Exception)
                    {
                        return false;   
                    }
                }
            }
            return true;
        }

        // MD5 加密
        private string EncryptPassword(string password)
        {
            MD5 pswMd5 = MD5.Create();
            byte[] buffer = Encoding.UTF8.GetBytes(password);
            byte[] data = pswMd5.ComputeHash(buffer);

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();

        }


        // TODO:UpdateUserInfo
        partial void UpdateUserInfo();

        // TODO:DeleteUserInfo
        partial void DeleteUserInfo();
    }
}
