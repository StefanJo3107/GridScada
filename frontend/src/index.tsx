import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import axios from 'axios';
import { createTheme, ThemeProvider } from '@mui/material';
import { createBrowserRouter, Navigate, RouterProvider } from 'react-router-dom';
import {Toaster} from "react-hot-toast";
import { AuthProvider } from './utils/AuthContext';
import Login from './pages/Login';
import { UnauthenticatedRoute } from './utils/UnauthenticatedRoute';
import { AuthenticatedRoute } from './utils/AuthenticatedRoute';
import { AdminRoute } from './utils/AdminRoute';
import Register from './pages/Register';
import Navbar from './components/Navbar';
import TagsCRUD from './pages/TagsCRUD';
import Tags from './pages/Tags';
import Reports from './pages/Reports';


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
    {path:"/register", element: <AuthenticatedRoute><AdminRoute><Navbar/><Register/></AdminRoute></AuthenticatedRoute>},
    {path:"/manager", element: <AuthenticatedRoute><AdminRoute><Navbar/><TagsCRUD/></AdminRoute></AuthenticatedRoute>},
    {path:"/tags", element: <AuthenticatedRoute><Navbar/><Tags/></AuthenticatedRoute>},
    {path:"/reports", element: <AuthenticatedRoute><AdminRoute><Navbar/><Reports/></AdminRoute></AuthenticatedRoute>},
    {path:"*", element: <Navigate to="/tags" replace />},
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
