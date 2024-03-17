import * as userContain from "../constants/userConstain";

//Login
export const userLoginReducer = (state = {}, action) => {
  switch (action.type) {
    case userContain.USER_LOGIN_REQUEST:
      return { isLoading: true };
    case userContain.USER_LOGIN_SUCCESS:
      return { isLoading: false, userInfo: action.payload, isSuccess: true };
    case userContain.USER_LOGIN_FAIL:
      return { isLoading: false, isError: action.payload };
    case userContain.USER_LOGIN_RESET:
      return {};
    case userContain.USER_LOGOUT:
      return {};
    default:
      return state;
  }
};


//Resgister
export const userRegisternReducer = (state = {}, action) => {
  switch (action.type) {
    case userContain.USER_REGISTER_REQUEST:
      return { isLoading: true };
    case userContain.USER_REGISTER_SUCCESS:
      return { isLoading: false, userInfo: action.payload, isSuccess: true };
    case userContain.USER_REGISTER_FAIL:
      return { isLoading: false, isError: action.payload };
    case userContain.USER_REGISTER_RESET:
      return {};
    default:
      return state;
  }
};