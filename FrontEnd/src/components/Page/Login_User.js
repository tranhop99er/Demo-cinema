import React, { useState, useEffect } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { GoogleOutlined, FacebookOutlined } from "@ant-design/icons";
import { Modal } from "react-bootstrap";
import axios from "axios";
import { useDispatch, useSelector } from "react-redux";

const Login_User = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();

const { isLoading, isError, userInfo, isSuccess} = useSelector((state) => state.userLogin);

  const [showModal, setShowModal] = useState(false);
  const handleModalOpen = () => setShowModal(true);
  const handleModalClose = () => setShowModal(false);

  //Sign in
  const [dataInputLogin, setDataInputLogin] = useState({
    email: "",
    password: "",
    accessToken: "",
  });

  const [verificationCode, setVerificationCode] = useState("");
  //handle signin
  const handleCheckVerifyCode = (e) => {
    if (verificationCode === "123215") {
      return true;
    } else {
      return false;
    }
  };

  //handle fotget pasword
  const [emailForget, setEmailForget] = useState("");
  const [verificationCodeForget, setVerificationCodeForget] = useState("");
  const handleForgetPassword = (e) => {
    e.preventDefault();
    if (verificationCodeForget === "123215") {
      console.log("Email quên mật khẩu:", emailForget);
    } else {
      console.log("Mã capcha sai. Nhập lại");
    }
    handleModalClose();
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (handleCheckVerifyCode()) {
      try {
        const { email, password, accessToken } = dataInputLogin;
        const response = await axios.post("https://localhost:7016/auth/login", {
          email,
          password,
        });

        const data = response.data;
        if (data.Status === 200) {
          // Lưu trữ token
          const userJSON = JSON.stringify(data); // lưu dữ liệu người dùng
          const token = JSON.stringify(data.Data.AccessToken); // lưu token vào để sau lấy dữ liệu sẽ cần phải dùng
          localStorage.setItem("user", userJSON);
          localStorage.setItem("token", token);
          // Chuyển hướng đến trang chủ
          alert(data.Message);
          navigate("/loginsuccess");
        }
      } catch (error) {
        // Xử lý lỗi, ví dụ: hiển thị thông báo lỗi hoặc ghi log
        alert("Email va kat khau sai. Nhap lai!");
      }
    } else {
      alert("Mã capcha sai. Nhập lại");
      // Add your logic for unsuccessful sign-in here
    }
  };

  return (
    <>
      <form onSubmit={handleSubmit} class="login-form">
        <div class="mb-3">
          <ul class="nav nav-tabs text-uppercase tab-information">
            <li className="text-center bg-primary" style={{ width: "50%" }}>
              <NavLink to="/login">
                <span className="nav-link text-white">Đăng nhập</span>
              </NavLink>
            </li>
            <li style={{ width: "50%" }} className="text-center list-unstyled">
              <NavLink to="/register" className="nav-link">
                <span>Đăng Ký</span>
              </NavLink>
            </li>
          </ul>
        </div>

        <div class="mb-3">
          <label for="exampleInputEmail1" class="form-label">
            Email
          </label>
          <input
            type="email"
            class="form-control"
            id="exampleInputEmail1"
            aria-describedby="emailHelp"
            placeholder="Email"
            value={dataInputLogin.email}
            onChange={(e) =>
              setDataInputLogin({ ...dataInputLogin, email: e.target.value })
            }
          />
        </div>
        <div class="mb-3">
          <label for="exampleInputPassword1" class="form-label">
            Mật khẩu
          </label>
          <input
            type="password"
            class="form-control"
            id="exampleInputPassword1"
            placeholder="Mật khẩu"
            value={dataInputLogin.password}
            onChange={(e) =>
              setDataInputLogin({ ...dataInputLogin, password: e.target.value })
            }
          />
        </div>
        <div class="mb-3 form-check">
          {/* <input type="checkbox" class="form-check-input" id="exampleCheck1" /> */}
          <NavLink to="#" onClick={handleModalOpen}>
            <label class="form-check-label text-info" for="exampleCheck1">
              Quên mật khẩu?
            </label>
          </NavLink>
        </div>

        <Modal show={showModal} onHide={handleModalClose}>
          <Modal.Header closeButton>
            <Modal.Title>Lấy lại mật khẩu</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            {/* Modal content goes here */}
            <form>
              <div class="mb-3">
                <label for="exampleInputEmail1" class="form-label">
                  Email
                </label>
                <input
                  type="email"
                  class="form-control"
                  id="exampleInputEmail1"
                  aria-describedby="emailHelp"
                  value={emailForget}
                  onChange={(e) => setEmailForget(e.target.value)}
                />
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
                    class="form-control"
                    id="exampleInputPassword2"
                    placeholder="Nhập mã xác thực"
                    value={verificationCodeForget}
                    onChange={(e) => setVerificationCodeForget(e.target.value)}
                  />
                </div>
              </div>
              <div class="text-center mb-3">
                <button
                  class="btn btn-outline-primary mx-auto"
                  onClick={handleForgetPassword}
                >
                  Lấy lại mật khẩu
                </button>
              </div>
            </form>
          </Modal.Body>
        </Modal>

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
              class="form-control"
              id="exampleInputPassword2"
              placeholder="Nhập mã xác thực"
              value={verificationCode}
              onChange={(e) => setVerificationCode(e.target.value)}
            />
          </div>
        </div>

        <div class="text-center mb-3">
          <button
            type="submit"
            class="btn btn-outline-primary mx-auto"
            // onClick={handleClickSignIn}
          >
            Đăng nhập
          </button>
        </div>
        <div class="row mb-3">
          <div class="col-md-6">
            <div class="text-center mb-3">
              <button class="btn btn-warning mx-auto">
                <GoogleOutlined
                  style={{ fontSize: "18px", marginRight: "4px" }}
                />
                Đăng nhập bằng Google
              </button>
            </div>
          </div>
          <div class="col-md-6">
            <div class="text-center mb-3">
              <button class="btn btn-primary mx-auto">
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

export default Login_User;
