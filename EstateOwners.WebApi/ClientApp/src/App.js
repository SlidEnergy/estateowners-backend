import React from 'react';
import AppRoutes from './routing/AppRoutes';
import Layout from './pages/layout/Layout';
import './custom.css';
import AuthProvider from "./core/auth/AuthProvider";

export const App = () => {
    return (
        <AuthProvider>
            <Layout>
                <AppRoutes></AppRoutes>
            </Layout>
        </AuthProvider>
    );
}

export default App;