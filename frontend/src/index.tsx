import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import axios from 'axios';
import { createTheme, ThemeProvider } from '@mui/material';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import {Toaster} from "react-hot-toast";
import { AuthProvider } from './utils/AuthContext';
import Login from './pages/Login';
import { UnauthenticatedRoute } from './utils/UnauthenticatedRoute';


axios.defaults.withCredentials = true

const theme = createTheme({
    palette: {
        primary: {
            main: "#0f0b0a",
        },
        secondary: {
            main: "#fdefc7",
            contrastText: 'white'
        },
    },
});

const router = createBrowserRouter([
    {path:"/login", element: <UnauthenticatedRoute><Login/></UnauthenticatedRoute>},
])


ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  //<React.StrictMode>
      <ThemeProvider theme={theme} >
          <AuthProvider>
            <RouterProvider router={router}/>
            <Toaster position="bottom-right"/>
          </AuthProvider>
      </ThemeProvider>
  //</React.StrictMode>,
)
