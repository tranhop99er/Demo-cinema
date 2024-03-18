
import { useEffect } from 'react';
import './App.css';
import Footer from './components/Footer/Footer';
import Topbar from './components/Header/TopBar';
import AppRouter from './components/Routes/AppRouter';
import { useDispatch, useSelector } from "react-redux";
import { userActions } from './components/store';

function App() {
  const dispatch = useDispatch();
  const isUserLoggedIn = useSelector((state) => state.user.isLoggedIn);
  // console.log(isUserLoggedIn);
  useEffect(() => {
    if(localStorage.getItem("userName")){
      dispatch(userActions.login());
    }
  }, [])
  return (
    <div className="App">
      <Topbar />
      <AppRouter />
      <Footer />
    </div>
  );
}

export default App;
