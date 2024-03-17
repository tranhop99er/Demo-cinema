
import Axios from "./Axios";

//Register a new user
const registerService = async (user) => {
    const {data} = await Axios.post("/auth/register", user);
    if(data){
        localStorage.setItem("userInfo", JSON.stringify(data));
        return data;
    }
}

//LogOut user
const logoutService = () => {
    localStorage.removeItem("userInfo")
    return null
}


//Login user
const loginService = async (user) => {
    const { data } = await Axios.post("/auth/login", user);
    if(data){
        localStorage.setItem("userInfo", JSON.stringify(data));
        return data;
    }
}

export { registerService , logoutService, loginService};