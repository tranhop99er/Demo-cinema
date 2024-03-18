import { NavLink } from "react-router-dom";
import "./Header.css";
import { useDispatch, useSelector } from "react-redux";
import { Modal } from "react-bootstrap";
import { useState } from "react";
import { userActions } from "../store";
import axios from "axios";

const Header = () => {
  const dispatch = useDispatch();
  const isUserLoggedIn = useSelector((state) => state.user.isLoggedIn);

  const [showDropdown, setShowDropdown] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const handleModalOpen = () => setShowModal(true);
  const handleModalClose = () => setShowModal(false);

  const logOut = () => {
    dispatch(userActions.logout());
  };
  const userName = localStorage.getItem("userName");
  const OldPassword = localStorage.getItem("password");
  const [dataInputChangePassWord, setDataInputChangePassWord] = useState({
    oldPassword: "",
    newPassword: "",
    newPasswordConfirm: "",
  });

  //Change password
  const handleSubmitChangePassword = async (event) => {
    event.preventDefault();

    // {console.log(dataInputChangePassWord)}
    if (
      dataInputChangePassWord.oldPassword === OldPassword &&
      dataInputChangePassWord.newPassword ===
        dataInputChangePassWord.newPasswordConfirm
    ) {
      const dataToBackend = {
        Username: userName,
        OldPassword: OldPassword,
        NewPassword: dataInputChangePassWord.newPassword,
      };
      console.log(dataToBackend);
      const urlChangePassWord = "https://localhost:7016/changePassword";
      axios
        .put(urlChangePassWord, dataToBackend, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          {
            console.log(response);
          }
          alert(response.data);
        })
        .catch((error) => {
          alert(error);
        });
        localStorage.setItem("password", dataInputChangePassWord.newPassword);
        setDataInputChangePassWord({
          oldPassword: "",
          newPassword: "",
          newPasswordConfirm: "",
        });
    } else {
      alert("Nhập mật khẩu sai. Nhập lại");
    }
  };

  return (
    <div className="appHeader">
      <div className="topHeader">
        {!isUserLoggedIn && (
          <>
            <NavLink to="/Login" className="headerLink">
              <span>Đăng nhập</span>
            </NavLink>
            <span className="divider">|</span>
            <NavLink to="/register" className="headerLink">
              <span>Đăng Ký</span>
            </NavLink>
          </>
        )}

        {isUserLoggedIn && (
          <>
            <p
              className="headerLink"
              onClick={() => setShowDropdown(!showDropdown)}
            >
              Xin chào: {userName} <span className="fa fa-angle-down"></span>
            </p>
            {showDropdown && (
              <form
                onSubmit={handleSubmitChangePassword}
                class="changePassword-form"
              >
                <div className="dropdown-menu show dropdown-itemInfo">
                  <a className="dropdown-item" href="#/action-1">
                    Thông tin tài khoản
                  </a>
                  <a onClick={handleModalOpen} className="dropdown-item">
                    Đổi mật khẩu
                  </a>
                  <a
                    onClick={() => logOut()}
                    className="dropdown-item"
                    href="/"
                  >
                    Đăng xuất
                  </a>
                  <Modal show={showModal} onHide={handleModalClose}>
                    <Modal.Header closeButton>
                      <Modal.Title>Đổi mật khẩu</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                      {/* Modal content goes here */}
                      <form>
                        <div class="mb-3">
                          <label for="exampleInputEmail1" class="form-label">
                            Mật khẩu cũ
                          </label>
                          <input
                            type="tel"
                            class="form-control"
                            id="exampleInputpassword"
                            aria-describedby="emailHelp"
                            value={dataInputChangePassWord.oldPassword}
                            onChange={(e) =>
                              setDataInputChangePassWord({
                                ...dataInputChangePassWord,
                                oldPassword: e.target.value,
                              })
                            }
                          />
                        </div>
                        <div class="mb-3">
                          <label for="exampleInputEmail1" class="form-label">
                            Mật khẩu mới
                          </label>
                          <input
                            type="tel"
                            class="form-control"
                            id="exampleInputpassword"
                            aria-describedby="emailHelp"
                            value={dataInputChangePassWord.newPassword}
                            onChange={(e) =>
                              setDataInputChangePassWord({
                                ...dataInputChangePassWord,
                                newPassword: e.target.value,
                              })
                            }
                          />
                        </div>
                        <div class="mb-3">
                          <label for="exampleInputEmail1" class="form-label">
                            Xác nhận mật khẩu mới
                          </label>
                          <input
                            type="tel"
                            class="form-control"
                            id="exampleInputpassword"
                            aria-describedby="emailHelp"
                            value={dataInputChangePassWord.newPasswordConfirm}
                            onChange={(e) =>
                              setDataInputChangePassWord({
                                ...dataInputChangePassWord,
                                newPasswordConfirm: e.target.value,
                              })
                            }
                          />
                        </div>
                        <div class="text-center mb-3">
                          <button
                            type="submit"
                            class="btn btn-outline-primary mx-auto"
                            onClick={handleModalClose}
                          >
                            Xác nhận
                          </button>
                        </div>
                      </form>
                    </Modal.Body>
                  </Modal>
                </div>
              </form>
            )}
          </>
        )}
      </div>
    </div>
  );
};

export default Header;
