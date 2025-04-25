import { createBrowserRouter } from 'react-router-dom';
import App from '../App.tsx';
import { Home } from '../features/Home/Home.tsx';
import { Login } from './Login/Login.tsx';
import { UserCreate } from './Login/UserCreate.tsx';
import { BusinessCreate } from './Login/BusinessCreate.tsx';
import { PasswordRecoverForm } from './Login/PasswordRecover.tsx';
import { Clients } from '../features/Clients/Clients.tsx';
import { Patients } from '../features/Clients/Patients/Patients.tsx';
import { Measurement } from '../features/AdhesionMeasurement/Measurement.tsx';
import { QuarterlyDetail } from '../features/DataCuts/QuarterlyDetail.tsx';
import { MeasurementDetail } from '../features/AdhesionMeasurement/MeasurementDetail.tsx';
import { Functionary } from '../features/Clients/Professionals/Functionary.tsx';
import { Guide } from '../features/Guide/Guide.tsx';
import { GuideDetail } from '../features/Guide/GuideDetail.tsx';
import { Settings, } from '../features/Settings/Settings.tsx';
import { Users } from '../features/Settings/Users/Users.tsx';
import { Scales } from '../features/Settings/Scales/Scales.tsx';
import { Roles } from '../features/Settings/Roles/Roles.tsx';
import { DataCuts } from '../features/DataCuts/DataCuts.tsx';
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
                path: '/Dashboard',
                element: <Home />,
            },
            {
                path: '/Clients',
                element: <Clients />,
            },

            {
                path: '/Patients',
                element: <Patients />,
            },

            {
                path: '/Functionary',
                element: <Functionary />,
            },

            {
                path: '/Guide',
                element: <Guide />,
            },
            {
                path: 'Guide/:id',
                element: <GuideDetail />,
            },

            {
                path: '/DataCuts',
                element: <DataCuts />,
            },

            {
                path: 'Quarterly/:Id',
                element: <QuarterlyDetail />,
            },

            {
                path: '/Measurement',
                element: <Measurement />,
            },

            {
                path: 'AdhesionMeasurement/:id',
                element: <MeasurementDetail />,
            },

            {
                path: '/Settings',
                element: <Settings />,
            },

            {
                path: '/Users',
                element: <Users />,
            },

            {
                path: '/Scales',
                element: <Scales />,
            },

            {
                path: '/Roles',
                element: <Roles />,
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
