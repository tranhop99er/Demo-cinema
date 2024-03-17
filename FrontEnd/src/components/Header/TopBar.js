import React, { useState } from "react";
import "./TopBar.css";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import Container from "react-bootstrap/Container";
import betaLogo from "../../assets/betaLogo.jpg";
import Dropdown from "react-bootstrap/Dropdown";
import { NavLink, useNavigate } from "react-router-dom";
import { loginBool } from "../Page/Login";

const Topbar = () => {
  const [loginBoolPage, setLoginBoolPage] = useState(loginBool);
  const navigate = useNavigate();
  const handleSignOut = () => {
    setLoginBoolPage(false);
    navigate("/");
  };

  return (
    <div>
      <Navbar
        expand="lg"
        className="bg-light shadow-sm nav-bard topBarHeader"
        style={{ height: "50px" }}
      >
        <Container>
          <NavLink to="/">
            <img
              src={betaLogo}
              alt="logo"
              style={{ width: "40%", height: "50%", objectFit: "cover" }}
            />
          </NavLink>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <div class="address">
              <select
                class="form-control text-center"
                id="gender"
                name="gender"
                required
                // style={{ height: "30px" }}
              >
                <option value="male" style={{ fontSize: "12px" }}>
                  Beta Thái Nguyên
                </option>
                <option value="male" style={{ fontSize: "12px" }}>
                  Hà Nội
                </option>
                <option value="female" style={{ fontSize: "12px" }}>
                  Vinh
                </option>
                <option value="other" style={{ fontSize: "12px" }}>
                  Tp. Hồ Chí Minh
                </option>
              </select>
            </div>

            <Nav className="ms-auto text-center d-flex justify-content-center align-items-center">
              <NavLink to="/home" className="nav-link text-secondary fw-bold">
                Lịch chiếu theo rạp
              </NavLink>
              <NavLink to="/products" className="text-secondary fw-bold nav-link">
                Phim
              </NavLink>
              <NavLink to="/home" className="nav-link text-secondary fw-bold">
                Rạp
              </NavLink>
              <NavLink to="/home" className="nav-link text-secondary fw-bold">
                Giá vé
              </NavLink>
              <NavLink to="/home" className="nav-link text-secondary fw-bold">
                Tin mới và ưu đãi
              </NavLink>
              <NavLink to="/home" className="nav-link text-secondary fw-bold">
                Nhượng quyền
              </NavLink>
              <NavLink to="/login" className="nav-link text-secondary fw-bold">
                Thành viên
              </NavLink>
              {loginBoolPage ? (
                <Dropdown>
                  <Dropdown.Toggle variant="success" id="dropdown-basic">
                    Avatar
                  </Dropdown.Toggle>

                  <Dropdown.Menu>
                    <Dropdown.Item href="#/action-1">
                      Thông tin tài khoản
                    </Dropdown.Item>
                    <Dropdown.Item href="#/action-2">
                      Đổi mật khẩu
                    </Dropdown.Item>
                    {console.log(loginBoolPage)}
                    <Dropdown.Item onClick={handleSignOut}>
                      Đăng xuất
                    </Dropdown.Item>
                  </Dropdown.Menu>
                </Dropdown>
              ) : (
                <>
                  <NavLink
                    to="/loginUser"
                    className="nav-link text-secondary fw-bold text-decoration-underline"
                  >
                    Đăng nhập
                  </NavLink>
                  <NavLink
                    to="/register"
                    className="nav-link text-secondary fw-bold text-decoration-underline"
                  >
                    Đăng ký
                  </NavLink>
                </>
              )}
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
    </div>
  );
};

export default Topbar;
