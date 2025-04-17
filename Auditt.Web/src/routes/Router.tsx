import { createBrowserRouter } from 'react-router-dom';
import App from '../App.tsx';
import { Home } from '../features/Home/Home.tsx';
import { Login } from './Login/Login.tsx';
import { UserCreate } from './Login/UserCreate.tsx';
import { BusinessCreate } from './Login/BusinessCreate.tsx';
import { PasswordRecoverForm } from './Login/PasswordRecover.tsx';
import { Clients } from '../features/Clients/Clients.tsx';
import { Patients } from '../features/Clients/Patients/Patients.tsx';
import { Professionals } from '../features/Clients/Patients/Professionals/Professionals.tsx';
import { Instruments } from '../features/Instruments/Instruments.tsx';
import { Quarterly } from '../features/QuarterlyCuts/Quarterly.tsx';
import { Measurement } from '../features/AdhesionMeasurement/Measurement.tsx';
import { InstrumentsDetail } from '../features/Instruments/InstrumentsDetail.tsx';
import {  QuarterlyDetail } from '../features/QuarterlyCuts/QuarterlyDetail.tsx';
import { MeasurementDetail } from '../features/AdhesionMeasurement/MeasurementDetail.tsx';
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
                path: 'Instruments/:id',
                element: <InstrumentsDetail/>,
            },

            {
                path: '/Quarterly',
                element: <Quarterly/>,
            },

            {
                path: 'Quarterly/:Id',
                element: <QuarterlyDetail/>,
            },

            {
                path: '/Measurement',
                element: <Measurement/>,
            },

            {
                path: 'AdhesionMeasurement/:id',
                element: <MeasurementDetail/>,
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
