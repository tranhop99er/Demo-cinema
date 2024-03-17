import * as userContains from "../constants/userConstain"
import * as userApi from "../API/userServices"
import { ErrorsAction } from "../Protection";

//Login action
const loginAction = (datas) => async (dispatch) => {
    try {
        dispatch({ type: userContains.USER_LOGIN_REQUEST});
        const response = await userApi.loginService(datas);
        dispatch({ type: userContains.USER_LOGIN_SUCCESS, payload: response });
    } catch (error) 
    {
        ErrorsAction(error, dispatch, userContains.USER_LOGIN_FAIL)
    }
}


//Register action
const registerAction = (datas) => async (dispatch) => {
    try {
        dispatch({ type: userContains.USER_REGISTER_REQUEST});
        const response = await userApi.registerService(datas);
        dispatch({ type: userContains.USER_REGISTER_SUCCESS, payload: response });
        dispatch({ type: userContains.USER_LOGIN_SUCCESS, payload: response });
    } catch (error) 
    {
        ErrorsAction(error, dispatch, userContains.USER_REGISTER_FAIL)
    }
}


//Logout action
const logoutAction = () => async (dispatch) => {
    userApi.loginService()
    dispatch({ type: userContains.USER_LOGOUT });
    dispatch({ type: userContains.USER_LOGIN_RESET });
    dispatch({ type: userContains.USER_REGISTER_RESET });
}


export { loginAction, registerAction, logoutAction}