import { Route, Routes } from "react-router-dom";
import Login from "../Page/Login";
import Register from "../Page/Register";
import Home from "../Home/Home";
import Product from "../Products/Product";
import LoginSuccess from "../Page/LoginSuccess";
import Login_User from "../Page/Login_User";
import Products from "../Products/Products";

const AppRouter = () => {
  return (
    <div className="mt-5">
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/loginUser" element={<Login_User />} />
        <Route path="/loginsuccess" element={<LoginSuccess />} />
        <Route path="/register" element={<Register />} />
        <Route path="/products" element={<Products />} />
        <Route path="/products/:id" element={<Product />} />
      </Routes>
    </div>
  );
};

export default AppRouter;
