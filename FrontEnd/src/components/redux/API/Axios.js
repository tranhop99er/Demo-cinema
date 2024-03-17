import axios from "axios";

const Axios = axios.create({
    baseURL : "https://localhost:7016"
})

export default Axios;