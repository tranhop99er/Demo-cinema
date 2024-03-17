import { logoutAction } from "./action/userAction";

export const ErrorsAction = (error, dispatch, action) => {
  const message =
    error.response && error.response.data.message
      ? error.response.data.message
      : error.message;
  if (message === "Dang nhap that bai") {
    dispatch(logoutAction());
  }
  return dispatch({ type: action, payload: message });
};
