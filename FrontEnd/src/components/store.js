import { configureStore, createSlice } from "@reduxjs/toolkit";

const userSclice = createSlice({
  name: "user",
  initialState: { isLoggedIn: false },
  reducers: {
    login(state) {
      state.isLoggedIn = true;
    },
    logout(state) {
      localStorage.removeItem("userName");
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      localStorage.removeItem("password");
      state.isLoggedIn = false;
    },
  },
});

export const userActions = userSclice.actions;

export const store = configureStore({
  reducer: {
    user: userSclice.reducer,
  },
});
