import { NavLink } from "react-router-dom";

const Header = () => {
  return (
    <div className="appHeader">
      <div className="topHeader">
        <NavLink to="/login" className="headerLink">
          <span>Đăng nhập</span>
        </NavLink>
        <span className="divider">|</span>
        <NavLink to="/register" className="headerLink">
          <span>Đăng Ký</span>
        </NavLink>
      </div>
    </div>
  );
};

export default Header;
