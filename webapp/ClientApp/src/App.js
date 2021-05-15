import React, { useState } from 'react';
import { Home } from './components/Home';

import './custom.css'
import { AnalysisResult } from './components/AnalysisResult';

const App = () => {
    const [triggeredAnalysis, setTriggeredAnalysis] = useState(false);
    return (
        <div style={{height: '100%'}}>
            {!triggeredAnalysis && <Home triggerAnalysis={() => setTriggeredAnalysis(true)} />}
            {triggeredAnalysis && <AnalysisResult />}
        </div>
    );
}

export default App
