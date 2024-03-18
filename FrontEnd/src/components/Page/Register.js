import React, { useState } from "react";
import { NavLink } from "react-router-dom";
import { GoogleOutlined, FacebookOutlined } from "@ant-design/icons";
import axios from "axios";

const Register = () => {
  //Sign up
  const [formData, setFormData] = useState({
    fullName: "",
    userName: "",
    email: "",
    password: "",
    confirmPassword: "",
    dateOfBirth: "",
    gender: "male", // Giới tính mặc định là Nam
    phoneNumber: "",
    verificationCode: "",
    agreeTerms: false, // Chưa đồng ý điều khoản ban đầu
  });

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData((prevData) => ({
      ...prevData,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleClickSignUp = (e) => {
    e.preventDefault();
    const dataToBackend = {
      Username: formData.userName,
      Email: formData.email,
      Name: formData.fullName,
      PhoneNumber: formData.phoneNumber,
      Password: formData.password,
    };

    console.log(dataToBackend)
    const urlRegister = "https://localhost:7016/auth/register";
    axios
      .post(urlRegister, dataToBackend, {
        headers: {
          "Content-Type": "application/json",
        },
      })
      .then((response) => {
        {console.log(response);}
        alert(response.data.Message);
      }).catch((error) => {
        alert(error)
      })
  };


  return (
    <>
      <form class="login-form">
        <div class="row mb-3">
          <div class="mb-3">
            <ul class="nav nav-tabs text-uppercase tab-information">
              <li
                className="text-center list-unstyled"
                style={{ width: "50%" }}
              >
                <NavLink to="/Login" className="nav-link">
                  <span>Đăng nhập</span>
                </NavLink>
              </li>
              <li
                style={{ width: "50%" }}
                className="text-center text-white bg-primary list-unstyled"
              >
                <NavLink to="/register">
                  <span className="nav-link text-white">Đăng Ký</span>
                </NavLink>
              </li>
            </ul>
          </div>
          <div class="col-md-6">
            <label for="exampleInputEmail1" class="form-label">
              Họ tên
            </label>
            <input
              className="form-control"
              name="fullName"
              value={formData.fullName}
              onChange={handleChange}
              required
            />
          </div>
          <div class="col-md-6">
            <label for="exampleInputEmail1" class="form-label">
              Email
            </label>
            <input
              className="form-control"
              id="exampleInputEmail1"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>
        </div>

        <div class="row mb-3">
          {" "}
          <div class="col-md-6">
            <label htmlFor="exampleUserName" className="form-label">
              UserName
            </label>
            <input
              type="tel"
              className="form-control"
              id="exampleUserName"
              name="userName"
              value={formData.userName}
              onChange={handleChange}
              required
            />
          </div>
          <div class="col-md-6">
            <label htmlFor="exampleInputPhoneNumber" className="form-label">
              Số điện thoại
            </label>
            <input
              type="tel"
              className="form-control"
              id="exampleInputPhoneNumber"
              name="phoneNumber"
              value={formData.phoneNumber}
              onChange={handleChange}
              required
            />
          </div>
        </div>

        <div class="row mb-3">
          <div class="col-md-6">
            <label for="exampleInputEmail1" class="form-label">
              Mật khẩu
            </label>
            <input
              type="password"
              className="form-control"
              id="exampleInputPassword1"
              name="password"
              placeholder="Mật khẩu"
              value={formData.password}
              onChange={handleChange}
              required
            />
          </div>
          <div class="col-md-6">
            <label for="exampleInputPasword2" class="form-label">
              Nhập lại mật khẩu
            </label>
            <input
              type="password"
              className="form-control"
              id="exampleInputPassword1"
              name="confirmPassword"
              placeholder="Xác nhận lại mật khẩu"
              value={formData.confirmPassword}
              onChange={handleChange}
              required
            />
          </div>
        </div>

        <div class="row mb-3">
          <div class="col-md-6">
            <label for="exampleInputEmail1" class="form-label">
              Ngày sinh
            </label>
            <input
              type="date"
              className="form-control"
              name="dateOfBirth"
              value={formData.dateOfBirth}
              onChange={handleChange}
              required
            />
          </div>
          <div class="col-md-6">
            <label for="exampleInputEmail1" class="form-label">
              Giới tính
            </label>
            <select
              className="form-control"
              id="gender"
              name="gender"
              value={formData.gender}
              onChange={handleChange}
              required
            >
              <option value="male">Nam</option>
              <option value="female">Nữ</option>
              <option value="other">Khác</option>
            </select>
          </div>
        </div>

        <div class="row mb-3">
          <div class="col-md-6">
            <input
              class="form-control"
              id="exampleInputPassword1"
              value={123215}
            />
          </div>
          <div class="col-md-6">
            <input
              className="form-control"
              id="exampleInputVerificationCode"
              name="verificationCode"
              placeholder="Nhập mã xác thực"
              value={formData.verificationCode}
              onChange={handleChange}
              required
            />
          </div>
        </div>

        <div class="mb-3 form-check">
          <input
            type="checkbox"
            className="form-check-input"
            id="exampleCheck1"
            name="agreeTerms"
            checked={formData.agreeTerms}
            onChange={handleChange}
            required
          />
          <label class="form-check-label" for="exampleCheck1">
            Tôi cam kết tuân theo <NavLink>chính sách bảo mật</NavLink> và{" "}
            <NavLink>điều khoản sử dụng</NavLink> của BetaCinemas.
          </label>
        </div>

        <div class="text-center mb-3">
          <button
            type="submit"
            className="btn btn-outline-primary mx-auto"
            onClick={handleClickSignUp}
          >
            Đăng ký
          </button>
        </div>

        <div class="row mb-3">
          <div class="col-md-6">
            <div class="text-center mb-3">
              <button type="submit" class="btn btn-warning mx-auto">
                <GoogleOutlined
                  style={{ fontSize: "18px", marginRight: "4px" }}
                />
                Đăng nhập bằng Google
              </button>
            </div>
          </div>
          <div class="col-md-6">
            <div class="text-center mb-3">
              <button type="submit" class="btn btn-primary mx-auto">
                <FacebookOutlined
                  style={{ fontSize: "18px", marginRight: "4px" }}
                />
                Đăng nhập bằng Facebook
              </button>
            </div>
          </div>
        </div>
      </form>
    </>
  );
};

export default Register;
