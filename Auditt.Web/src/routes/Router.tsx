import { createBrowserRouter } from 'react-router-dom';
import App from '../App.tsx';
import { Home } from '../features/Home/Home.tsx';
import { Login } from './Login/Login.tsx';
import { UserCreate } from './Login/UserCreate.tsx';
import { BusinessCreate } from './Login/BusinessCreate.tsx';
import { PasswordRecoverForm } from './Login/PasswordRecover.tsx';
import { Clients } from '../features/Users/Clients.tsx';
import { Patients } from '../features/Users/Patients.tsx';
import { Professionals } from '../features/Users/Professionals/Professionals.tsx';
import { Instruments } from '../features/Instruments/Instruments.tsx';
import { QuarterlyCuts } from '../features/QuarterlyCuts/QuarterlyCuts.tsx';
export const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        children: [
            {
                index: true,
                element: <Home />,            
            },

            {
                path: '/Clients',
                element: <Clients/>,
            },

            {
                path: '/Patients',
                element: <Patients/>,
            },

            {
                path: '/Professionals',
                element: <Professionals/>,
            },

            {
                path: '/Instruments',
                element: <Instruments/>,
            },

            {
                path: '/QuarterlyCuts',
                element: <QuarterlyCuts/>,
            },

            

        ],
    },
    {
        path: 'Login',
        element: <Login />,
    },


    {
        path: '/Create/User',
        element: <UserCreate />,
    },
    {
        path: '/Create/Business',
        element: <BusinessCreate />,
    },
    {
        path: 'PasswordRecover',
        element: <PasswordRecoverForm />,
    },
]);
