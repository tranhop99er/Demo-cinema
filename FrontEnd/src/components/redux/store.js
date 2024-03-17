import { combineReducers } from "redux";
import { configureStore } from "@reduxjs/toolkit";
import * as User from "./Reducers/userReducers"
const rootReducers = combineReducers({
  //User reducer
  userLogin : User.userLoginReducer,
  userRegister : User.userRegisternReducer,
}); 

//get user info from localstorage
const userInfoFromStorage = localStorage.getItem("userInfo") 
  ? JSON.parse(localStorage.getItem("userInfo"))
  : null;


const initialState = {
  userLogin: {userInfo: userInfoFromStorage},
};

const store = configureStore({
  reducer: rootReducers,
  preloadedState: initialState,
});


export default store;